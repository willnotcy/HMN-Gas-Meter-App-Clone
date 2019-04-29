using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.OS;
using Android.Views;
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

        private Mat rgba;
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
                _openCvCameraView = null;
                _openCV = null;
                rgba = null;
                roi = null;
            }
        }
        private Rect GetRoi(Mat mat)
        {
            var boxWidth = mat.Width() - (mat.Width() / 3);
            var boxHeight = mat.Height() / 6;
            var scanBox = new Size(boxWidth, boxHeight);

            var xStartingPoint = mat.Width() / 6;
            var yStartingPoint = (mat.Height() / 2) - (mat.Height() / 10);
            var scanBoxStartingPoint = new Point(xStartingPoint,yStartingPoint);

            return new Rect(scanBoxStartingPoint, scanBox);
        }

        public Mat OnCameraFrame(Mat frame)
        {
            //Only done on first frame.
            //If ROI it not initailized, then we draw it on the frame.
            //However, if we have already drawn it on previous frame
            //then we don't need to draw it again.
            if(roi == null)
            {
                roi = GetRoi(frame);
            }

            // TODO: provide these in a global config file
            var contourMinHeight = frame.Height() / 34;
            var contourMaxHeight = frame.Height() / 4;

            // Initial rotation because camera starts in landscape mode.
            var input = _openCV.Rotate(frame, -90);


            rgba = input.Clone();
            var submat = input.Submat(roi);
            var submatClone = submat.Clone();

            rgba = _openCV.DrawRectangle(rgba, roi, new Scalar(0, 255, 0));

            // Turn image black and white.
            var gray = _openCV.ToGray(submat);

            // Brightness and contrast correction trough Histogram Equalization
            var equalized = _openCV.EqualizeHistogram(gray);

            // Blur image to reduce noise.
            var blur = _openCV.GaussianBlur(equalized);

            // Detect blobs using Otsu Tresholding
            var blobs = _openCV.OtsuThresh(blur);
            
            // Find contours in image. (clone because it this version of OpenCV applies some changes to the input image)
            var clone = blobs.Clone();
            var contours = _openCV.FindContours(clone);

            // Filter contours based on size -> then by Y position and height of their bounding boxes.
            var contoursBySize = _openCV.FilterContoursBySize(contours.Item1, contourMinHeight, contourMaxHeight);
            var alignedContours = _openCV.FilterContoursByYPosition(contoursBySize.Item1, contoursBySize.Item2);

            // Draw bounding boxes on input image for user visualization.
            var withBoundingBoxes = _openCV.DrawBoundingBoxes(submatClone.Clone(), alignedContours.Item2);
            withBoundingBoxes.CopyTo(rgba.Submat(roi));

            // Discard the frame if less than 8 matching contours are found. We want all the digits on the gas meter before processing.
            // TODO: if gasmeters have less than 8 digits, then what?
            if (alignedContours.Item2.Count != 8)
            {
                return rgba;
            }
       
            // Prepare output for OCR and stop the camera feed.
            try
            {
                var digits = new List<Stream>();
                var image = new Mat();
                var digitsClone = new List<Stream>();

                // Transform BGR image to RGB
                Imgproc.CvtColor(submatClone, submatClone, Imgproc.ColorBgr2rgba, 4);

                image = submatClone;

                // Sort digit bounding boxes left to right
                var sorted = _openCV.SortRects(alignedContours.Item2);

                // Cut each digit individually based on bounding box.
                foreach (Rect rect in sorted)
                {
                    digits.Add(_openCV.MatToStream(new Mat(blobs, rect)));
                    digitsClone.Add(_openCV.MatToStream(new Mat(blobs, rect)));
                }

                // TODO: Crop output image to region of interest when that is implemented.
                // Return digits and final image to display on confirmation page.
                MessagingCenter.Send(new CameraResultMessage { DigitsClone = digitsClone, Digits = digits, Image = _openCV.MatToStream(image) }, CameraResultMessage.Key);

                // Stop the camera feed and close the page
                input.Release();
                submat.Release();
                gray.Release();
                equalized.Release();
                blur.Release();
                blobs.Release();
                clone.Release();
                contours.Item2.Release();
                submatClone.Release();
                withBoundingBoxes.Release();
                image.Release();
                Finish();
            }
            catch (Exception)
            {
                // Processing the rectangles sometimes fail throwing an exception. These frames can discarded and the feed continues.
                return rgba;
            }

            return rgba;
        }

        public void OnCameraViewStarted(int width, int height)
        {
            rgba = new Mat(height, width, CvType.Cv8uc4);
        }

        public void OnCameraViewStopped()
        {
            rgba.Release();
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