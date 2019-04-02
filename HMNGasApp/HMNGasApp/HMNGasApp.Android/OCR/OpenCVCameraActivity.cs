using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HMNGasApp.Helpers;
using OpenCV.Android;
using OpenCV.Core;
using Xamarin.Forms;

namespace HMNGasApp.Droid.OCR
{
    [Activity(Label = "OCR-Scanning")]
    public class OpenCVCameraActivity : Activity, ILoaderCallbackInterface, CameraBridgeViewBase.ICvCameraViewListener
    {
        private CameraBridgeViewBase _openCvCameraView;
        private OpenCVServiceDroid _openCV { get; set; }

        private List<Stream> _digits;
        private Mat _image;

        private Mat mRgba;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            SetContentView(Resource.Layout.opencv_camera);
            _openCvCameraView = FindViewById<CameraBridgeViewBase>(Resource.Id.surfaceView);
            _openCvCameraView.Visibility = ViewStates.Visible;
            _openCvCameraView.SetCvCameraViewListener(this);
            _openCV = new OpenCVServiceDroid();
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (_openCvCameraView != null)
            {
                _openCvCameraView.DisableView();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!OpenCVLoader.InitDebug())
            {
                OpenCVLoader.InitAsync(OpenCVLoader.OpencvVersion300, this, this);
            }
            else
            {
                OnManagerConnected(LoaderCallbackInterface.Success);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_openCvCameraView != null)
            {
                _openCvCameraView.DisableView();
            }
        }

        public Mat OnCameraFrame(Mat p0)
        {
            // TODO: provide these in a global config file
            var cT1 = 140;
            var cT2 = cT1 * 2;
            var houghThresh = 350;
            var contourMinHeight = 55;
            var contourMaxHeight = 100;

            // Initial rotation because camera starts in landscape mode.
            var input = _openCV.Rotate(p0, -90);
            mRgba = input.Clone();

            // Turn image black and white.
            var gray = _openCV.ToGray(input);

            // Blur image to reduce noise.
            var blur = _openCV.MedianBlur(gray);

            // Detect edges using canny.
            var edges = _openCV.Canny(blur, cT1, cT2);

            // Detect straight lines using Hough Lines Transform.
            var lines = _openCV.HoughLines(edges, houghThresh);

            // Rotate image based on detected lines average gradient.
            var thetaDegrees = _openCV.GetAverageLineTheta(lines);
            var rotated = _openCV.Rotate(edges, thetaDegrees);

            // Find contours in image. (clone because it this version of OpenCV applies some changes to the input image)
            var clone = rotated.Clone();
            var contours = _openCV.FindContours(clone);

            // Filter contours based on size -> then by Y position and height of their bounding boxes.
            var contoursBySize = _openCV.FilterContoursBySize(contours.Item1, contourMinHeight, contourMaxHeight);
            var alignedContours = _openCV.FilterContoursByYPosition(contoursBySize.Item1, contoursBySize.Item2);

            // Draw bounding boxes on input image for user visualization.
            var withBoundingBoxes = _openCV.DrawBoundingBoxes(rotated.Clone(), alignedContours.Item2);
            mRgba = withBoundingBoxes;

            // Discard the frame if less than 8 matching contours are found. We want all the digits on the gas meter before processing.
            if(alignedContours.Item2.Count < 8)
            {
                return mRgba;
            }

            // Prepare output for OCR and stop the camera feed.
            try
            {
                _digits = new List<Stream>();
                _image = new Mat();

                _image = mRgba;

                // Sort digit bounding boxes left to right
                var sorted = _openCV.SortRects(alignedContours.Item2);

                // Cut each digit individually based on bounding box.
                foreach (Rect rect in sorted)
                {
                    _digits.Add(_openCV.MatToStream(new Mat(rotated, rect)));
                }

                // TODO: Crop output image to region of interest when that is implemented.
                // Return digits and final image to display on confirmation page.
                MessagingCenter.Send(new CameraResultMessage { Digits = _digits, Image = _openCV.MatToStream(_image) }, CameraResultMessage.Key);

                // Stop the camera feed and close the page
                Finish();
            }
            catch (Exception)
            {
                // Processing the rectangles sometimes fail throwing an exception. These frames can discarded and the feed continues.
                return mRgba;
            }

            return mRgba;
        }

        public void OnCameraViewStarted(int width, int height)
        {
            mRgba = new Mat(height, width, CvType.Cv8uc4);
        }

        public void OnCameraViewStopped()
        {
            mRgba.Release();
        }

        public void OnManagerConnected(int p0)
        {
            switch (p0)
            {
                case LoaderCallbackInterface.Success:
                    _openCvCameraView.EnableView();
                    break;
                default:
                    break;
            }
        }

        public void OnPackageInstall(int p0, IInstallCallbackInterface p1)
        {
            throw new NotImplementedException();
        }

        
    }
}