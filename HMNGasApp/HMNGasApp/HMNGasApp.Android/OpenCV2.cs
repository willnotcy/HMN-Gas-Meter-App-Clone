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

[assembly: Dependency(typeof(HMNGasApp.Droid.OpenCV2))]
namespace HMNGasApp.Droid
{
    public class OpenCV2 : IOpenCVService
    {
        private BaseLoaderCallback mLoaderCallback;

        public IOpenCVService OpenCVService { get; private set; }


        public void OpenCamera()
        {
            
        }

        public void Process()
        {
            throw new NotImplementedException();
        }

        public Stream Process(Stream image)
        {
            mLoaderCallback = new Callback(Android.App.Application.Context, this);
            if (!OpenCVLoader.InitDebug())
            {
                OpenCVLoader.InitAsync(OpenCVLoader.OpencvVersion300, Android.App.Application.Context, mLoaderCallback);
            }
            else
            {
                mLoaderCallback.OnManagerConnected(LoaderCallbackInterface.Success);
            }

            var mat = StreamToMat(image);

            //Image proccessing
            //TODO: make canny thresholds work for any contrasted image using histogram
            var cannyThreshold1 = 50;
            var cannyThreshold2 = 150;
            var houghLinesThreshold = 4;
            
            //Converting to GrayScale
            Mat gray = new Mat();
            Imgproc.CvtColor(mat, gray, Imgproc.ColorBgr2gray);
            
            //Detecting edges using Canny Algorithm
            Mat edges = new Mat();
            Imgproc.Canny(gray, edges, cannyThreshold1, cannyThreshold2);

            //Detect and correct remaining skew (+/- 30 deg)
            Mat lines = new Mat();
            Imgproc.HoughLinesP(edges, lines, 1, Math.PI / 180, 50, 50, 10);

            var t1 = edges.Type();
            var t2 = lines.Type();

            var c1 = lines.Cols();
            var c2 = edges.Cols();

            var r1 = lines.Rows();
            var r2 = edges.Rows();

            var cc1 = lines.Col(0);
            var cc2 = edges.Col(1);

            

            //Filter lines by theta and compute average
        
            Mat filteredLines = new Mat();
            double theta_min = 60 * Math.PI / 180;
            double theta_max = 120 * Math.PI / 180;
            double theta_avr = 0;
            double theta_deg = 0;
            /*
            for(int i = 0; i < lines.Cols(); i++ )
            {
                var wtf = lines.Get(1, i);
                double theta = lines.Get(1,i).FirstOrDefault();
                if (theta > theta_min && theta < theta_max)
                {
                    filteredLines.Push_back(lines.Row(i));
                    theta_avr += theta;
                }
            }

            if (filteredLines.Size().Area() > 0)
            {
                theta_avr /= (float) filteredLines.Size().Area();
                theta_deg = (theta_avr / (float) Math.PI * 180f) - 90;
            }
            */
            var stream = MatToStream(edges);
            return stream;
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

        public void SetInput()
        {
            throw new NotImplementedException();
        }
    }
}