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
            var cannyThreshold1 = 100;
            var cannyThreshold2 = 200;
            var houghLinesThreshold = 140;
            
            //Converting to GrayScale
            Mat gray = new Mat();
            Imgproc.CvtColor(mat, gray, Imgproc.ColorBgr2gray);
            
            //Detecting edges using Canny Algorithem;
            Mat edges = new Mat();
            Imgproc.Canny(gray, edges, cannyThreshold1, cannyThreshold2);

            //Detect and correct remaining skew (+/- 30 deg)
            Mat lines = new Mat();
            
            Imgproc.HoughLines(edges, lines, 1, Math.PI / 180f, houghLinesThreshold);


            //Filter lines by theta and compute average

            Mat filteredLines = new Mat();
            float theta_min = 60f * (float) Math.PI / 180f;
            float theta_max = 120f * (float) Math.PI / 180f;
            float theta_avr = 0f;
            float theta_deg = 0f;

            for(int i = 0; i < lines.Size().Area(); i++ )
            {
                float theta = (float) lines.Get(i,i)[1];
                if (theta > theta_min && theta < theta_max)
                {
                    filteredLines.Push_back(lines);
                    theta_avr += theta;
                }
            }

            if (filteredLines.Size().Area() > 0)
            {
                theta_avr /= (float) filteredLines.Size().Area();
                theta_deg = (theta_avr / (float) Math.PI * 180f) - 90;
            }
        
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
                if (Imgcodecs.Imencode(".jpg", m, vect))
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