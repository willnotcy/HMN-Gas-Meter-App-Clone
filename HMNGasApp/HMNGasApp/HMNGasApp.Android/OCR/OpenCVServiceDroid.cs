using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using HMNGasApp.Services;
using Xamarin.Forms;
using OpenCV.Android;
using System.IO;
using OpenCV.Core;
using OpenCV.ImgCodecs;
using OpenCV.ImgProc;
using Android.Graphics;
using Point = OpenCV.Core.Point;
using Rect = OpenCV.Core.Rect;
using HMNGasApp.Droid.OCR;
using Size = OpenCV.Core.Size;

[assembly: Dependency(typeof(HMNGasApp.Droid.OpenCVServiceDroid))]
namespace HMNGasApp.Droid
{
    public class OpenCVServiceDroid : IOpenCVService
    {
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
        #region Image processing

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
        public Mat OtsuThresh(Mat mat)
        {
            Mat output = new Mat();
            Imgproc.Threshold(mat, output,0,255,Imgproc.ThreshBinary+Imgproc.ThreshOtsu);
            return output;
        }
        public Mat GaussianBlur(Mat mat)
        {
            Mat output = new Mat();
            var kernel = new Size(3, 3);
            Imgproc.GaussianBlur(mat, output, kernel, 0);
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