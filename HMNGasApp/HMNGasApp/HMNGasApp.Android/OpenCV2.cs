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
            Mat mgray = new Mat();

            Imgproc.CvtColor(mat, mgray, Imgproc.ColorBgr2gray);

            var stream = MatToStream(mgray);
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