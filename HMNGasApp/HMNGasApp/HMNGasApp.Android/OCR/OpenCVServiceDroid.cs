using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HMNGasApp.Services;
using Xamarin.Forms;
using OpenCV;
using OpenCV.Android;
using System.IO;
using OpenCV.Core;
using OpenCV.ImgCodecs;
using OpenCV.ImgProc;
using Android.Graphics;
using System.Numerics;
using Point = OpenCV.Core.Point;
using Rect = OpenCV.Core.Rect;
using HMNGasApp.Droid.OCR;

[assembly: Dependency(typeof(HMNGasApp.Droid.OpenCVServiceDroid))]
namespace HMNGasApp.Droid
{
    public class OpenCVServiceDroid : IOpenCVService
    {
        private BaseLoaderCallback mLoaderCallback;

        #region Converter methods

        public Mat ToBGR(Mat mat)
        {
            Mat bgr = new Mat();
            Imgproc.CvtColor(mat, bgr, Imgproc.ColorGray2bgr);

            return bgr;
        }

        public Mat ToGray(Mat mat)
        {
            Mat gray = new Mat();
            Imgproc.CvtColor(mat, gray, Imgproc.ColorBgr2gray);

            return gray;
        }

        public Mat StreamToMat(Stream image)
        {

            var bmp = BitmapFactory.DecodeStream(image);

            Mat mat = new Mat();
            Bitmap bmp32 = bmp.Copy(Bitmap.Config.Argb8888, true);
            Utils.BitmapToMat(bmp32, mat);
            return mat;
        }

        public Stream MatToStream(Mat m)
        {
            using (var vect = new MatOfByte())
            {
                if (Imgcodecs.Imencode(".png", m, vect))
                {
                    var stream = new MemoryStream(vect.ToArray());
                    return stream;
                }
                return null;
            }
        }

        #endregion
        #region Image preprocessing

        /// <summary>
        /// Canny detector.
        /// Detects edges in image for gradients above thresh2 and between 
        /// thresh 1 and 2, if the pixel is connected to another pixel with
        /// a gradient value higher than thresh2.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="thresh1"></param>
        /// <param name="thresh2"></param>
        /// <returns>Mat with detected edges</returns>
        public Mat Canny(Mat mat, int thresh1, int thresh2)
        {
            Mat edges = new Mat();
            Imgproc.Canny(mat, edges, thresh1, thresh2, 3, false);

            return edges;
        }

        /// <summary>
        /// Blurs an image using the median filter to reduce noise. (Smoothing)
        /// </summary>
        /// <param name="mat"></param>
        /// <returns>Mat with applied filter</returns>
        public Mat MedianBlur(Mat mat)
        {
            Mat blur = new Mat();
            Imgproc.MedianBlur(mat, blur, 5);

            return blur;
        }

        /// <summary>
        /// The Hough Line Transform is a transform used to detect straight lines.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="thresh"></param>
        /// <returns>Mat of detected lines</returns>
        public Mat HoughLines(Mat mat, int thresh)
        {
            Mat lines = new Mat();
            Imgproc.HoughLines(mat, lines, 1, Math.PI / 180, thresh);

            return lines;
        }

        /// <summary>
        /// A more efficient implementation of the Hough Line Transform.
        /// It gives as output the extremes of the detected lines.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="thresh"></param>
        /// <param name="minLength"></param>
        /// <param name="maxGap"></param>
        /// <returns>Mat of detected lines</returns>
        public Mat HoughLinesP(Mat mat, int thresh, int minLength, int maxGap)
        {
            Mat lines = new Mat();
            Imgproc.HoughLinesP(mat, lines, 1, Math.PI / 180, thresh, minLength, maxGap);

            return lines;
        }

        /// <summary>
        /// Iterates over lines found with Hough Line Transform and finds the average theta degree.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>Average gradient</returns>
        public double GetAverageLineTheta(Mat lines)
        {
            Mat filteredLines = new Mat();

            double theta_min = 60 * Math.PI / 180;
            double theta_max = 120 * Math.PI / 180;
            double theta_avr = 0;
            double theta_deg = 0;

            for (int x = 0; x < lines.Rows(); x++)
            {
                double theta = lines.Get(x, 0)[1];
                if (theta > theta_min && theta < theta_max)
                {
                    filteredLines.Push_back(lines.Row(x));
                    theta_avr += theta;
                }
            }

            if (filteredLines.Rows() > 0)
            {
                theta_avr /= filteredLines.Rows();
                theta_deg = (theta_avr / Math.PI * 180f) - 90;
            }

            return theta_deg;
        }

        /// <summary>
        /// Rotates the image based on the rotationDegrees parameter.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="rotationDegrees"></param>
        /// <returns>Rotated Mat</returns>
        public Mat Rotate(Mat mat, double rotationDegrees)
        {
            Mat matrix = Imgproc.GetRotationMatrix2D(new Point(mat.Cols() / 2, mat.Rows() / 2), rotationDegrees, 1);

            Mat rotated = new Mat();
            Imgproc.WarpAffine(mat, rotated, matrix, mat.Size());

            return rotated;
        }

        /// <summary>
        /// Finds contours in given mat. Basically all found shapes.
        /// Uses RetrExternal to only provide outer bounds.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns>List of contours and their hierarchy</returns>
        public (IList<MatOfPoint>, Mat) FindContours(Mat mat)
        {
            Mat ret = mat.Clone();

            IList<MatOfPoint> contours = new JavaList<MatOfPoint>();
            List<MatOfPoint> filteredContours = new List<MatOfPoint>();
            Mat hierachy = new Mat();

            Imgproc.FindContours(mat, contours, hierachy, Imgproc.RetrExternal, Imgproc.ChainApproxNone);

            return (contours, hierachy);
        }

        /// <summary>
        /// Filters unwanted contours by checking their width and height based on defined min/max values.
        /// </summary>
        /// <param name="contours"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Filtered list of contours and their bounding boxes</returns>
        public (IList<MatOfPoint>, List<Rect>) FilterContoursBySize(IList<MatOfPoint> contours, int min, int max)
        {
            IList<MatOfPoint> filteredContours = new JavaList<MatOfPoint>();
            List<Rect> boundingBoxes = new List<Rect>();

            foreach (MatOfPoint contour in contours)
            {
                Rect bounds = Imgproc.BoundingRect(contour);
                if (bounds.Height > min && bounds.Height < max && bounds.Width > 10 && bounds.Width < bounds.Height)
                {
                    boundingBoxes.Add(bounds);
                    filteredContours.Add(contour);
                }
            }
            return (filteredContours, boundingBoxes);
        }

        /// <summary>
        /// Filters unwanted contours by checking the amount of similar contours on the same horizontal coordinates.
        /// </summary>
        /// <param name="contours"></param>
        /// <param name="bounds"></param>
        /// <returns>List of matching contours and their bounding boxes</returns>
        public (IList<MatOfPoint>, List<Rect>) FilterContoursByYPosition(IList<MatOfPoint> contours, List<Rect> bounds)
        {
            IList<MatOfPoint> alignedContours = new JavaList<MatOfPoint>();
            IList<MatOfPoint> tmpC;
            List<Rect> alignedBounds = new List<Rect>();
            List<Rect> tmpB;

            for (int i = 0; i < bounds.Count; i++)
            {
                Rect bound = bounds[i];
                tmpC = new JavaList<MatOfPoint>() { contours[i] };
                tmpB = new List<Rect>() { bound };

                for (int j = i + 1; j < bounds.Count; j++)
                {
                    Rect bound2 = bounds[j];
                    if (bound.Y - bound2.Y < 30 && bound2.X > bound.X + bound.Width)
                    {
                        tmpB.Add(bound2);
                        tmpC.Add(contours[j]);
                    }
                }

                if (tmpB.Count > alignedBounds.Count)
                {
                    alignedContours = tmpC;
                    alignedBounds = tmpB;
                }
            }

            return (alignedContours, alignedBounds);
        }
        public Mat EqualizeHistogram(Mat mat)
        {
            Mat output = new Mat();
            Imgproc.EqualizeHist(mat, output);
            return output;
        }
        public Mat AdaptiveThresh(Mat mat)
        {
            Mat output = new Mat();
            Imgproc.AdaptiveThreshold(mat, output, 255, Imgproc.AdaptiveThreshMeanC, Imgproc.ThreshBinary, 15, 40);
            return output;
        }
        #endregion
        #region Drawing methods

        /// <summary>
        /// Draws all given rectangles on the image in green.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="bounds"></param>
        /// <returns>Mat with drawn bounding boxes</returns>
        public Mat DrawBoundingBoxes(Mat mat, List<Rect> bounds)
        {
            foreach (Rect bound in bounds)
            {
                Imgproc.Rectangle(mat, new Point(bound.X, bound.Y), new Point(bound.X + bound.Width, bound.Y + bound.Height), new Scalar(0, 255, 0));
            }

            return mat;
        }
        public Mat DrawRectangle(Mat mat, Rect rect, Scalar colour)
        {
            Imgproc.Rectangle(mat, new Point(rect.X - 2, rect.Y - 2), new Point(rect.X + rect.Width + 2, rect.Y + rect.Height + 2), new Scalar(0, 255, 0));
            return mat;
        }
        #endregion
        #region Sort methods

        public List<Rect> SortRects(List<Rect> rects)
        {
            return rects.OrderBy(x => x.X).ToList();
        }

        #endregion


        public void OpenCamera()
        {
            var intent = new Intent(Android.App.Application.Context, typeof(OpenCVCameraActivity));
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}