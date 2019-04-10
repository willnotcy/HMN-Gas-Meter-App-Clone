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
using OpenCV.ImgProc;
using Xamarin.Forms;
using Point = OpenCV.Core.Point;
using Size = OpenCV.Core.Size;

namespace HMNGasApp.Droid.OCR
{
    [Activity(Label = "OCR-Scanning")]
    public class OpenCVCameraActivity : Activity, ILoaderCallbackInterface, CameraBridgeViewBase.ICvCameraViewListener
    {
        private CameraBridgeViewBase _openCvCameraView;
        private OpenCVServiceDroid _openCV { get; set; }

        private Mat mRgba;
        private Rect roi;

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
        private Rect GetRoi(Mat mat)
        {
            var scanBox = new Size((mat.Width() / 10) * 9,
                                   mat.Height() / 8);

            var scanBoxStartingPoint = new Point(mat.Width() / 20,
                                                 (mat.Height() / 2) - (mat.Height() / 10));

            return new Rect(scanBoxStartingPoint, scanBox);
        }

        public Mat OnCameraFrame(Mat p0)
        {
            if(roi == null)
            {
                roi = GetRoi(p0);
            }
            // TODO: provide these in a global config file
            var houghThresh = 350;
            var contourMinHeight = 60;
            var contourMaxHeight = 90;

            // Initial rotation because camera starts in landscape mode.
            var input = _openCV.Rotate(p0, -90);
            mRgba = input.Clone();
            var submat = input.Submat(roi);

            mRgba = _openCV.DrawRectangle(mRgba, roi, new Scalar(0, 255, 0));

            // Turn image black and white.
            var gray = _openCV.ToGray(submat);

            //Brightness and contrast correction
            var equalized = _openCV.EqualizeHistogram(gray);

            // Blur image to reduce noise.
            var blur = _openCV.GaussianBlur(equalized);

            // Detect edges using canny.
            var edges = _openCV.OtsuThresh(blur);

            //invert B/W
            Core.Bitwise_not(edges, edges);

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

            // Converts back to colour
            Imgproc.CvtColor(rotated, rotated, Imgproc.ColorGray2rgba, 4);

            // Draw bounding boxes on input image for user visualization.
            
            var withBoundingBoxes = _openCV.DrawBoundingBoxes(rotated.Clone(), alignedContours.Item2);
            withBoundingBoxes.CopyTo(mRgba.Submat(roi));


            // Discard the frame if less than 8 matching contours are found. We want all the digits on the gas meter before processing.
            if (alignedContours.Item2.Count != 8)
            {
                return mRgba;
            }

            // Prepare output for OCR and stop the camera feed.
            try
            {
                var digits = new List<Stream>();
                var image = new Mat();
                var digitsClone = new List<Stream>();

                image = rotated;

                // Sort digit bounding boxes left to right
                var sorted = _openCV.SortRects(alignedContours.Item2);

                // Cut each digit individually based on bounding box.
                foreach (Rect rect in sorted)
                {
                    digits.Add(_openCV.MatToStream(new Mat(rotated, rect)));
                    digitsClone.Add(_openCV.MatToStream(new Mat(rotated, rect)));
                }

                // TODO: Crop output image to region of interest when that is implemented.
                // Return digits and final image to display on confirmation page.
                MessagingCenter.Send(new CameraResultMessage { DigitsClone = digitsClone, Digits = digits, Image = _openCV.MatToStream(image) }, CameraResultMessage.Key);

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