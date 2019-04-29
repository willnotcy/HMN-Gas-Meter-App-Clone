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
using OpenCV.Android;
using OpenCV.Core;
using Android.Hardware;
using Android.Util;
using Java.IO;
using Size = Android.Hardware.Camera.Size;
using Java.Lang.Reflect;

namespace HMNGasApp.Droid.OCR
{
    public class OpenCVCameraView : JavaCameraView, Camera.IPictureCallback
    {
        private const string TAG = "CameraControlView";
        private String mPictureFileName;

        public OpenCVCameraView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public IList<String> GetEffectList()
        {
            return MCamera.GetParameters().SupportedColorEffects;
        }

        public bool IsEffectSupported()
        {
            return (MCamera.GetParameters().ColorEffect != null);
        }

        public String GetEffect()
        {
            return MCamera.GetParameters().ColorEffect;
        }

        public void SetEffect(String effect)
        {
            Camera.Parameters param = MCamera.GetParameters();
            param.ColorEffect = effect;
            MCamera.SetParameters(param);
        }

        public List<Size> GetResolutionList()
        {
            return MCamera.GetParameters().SupportedPreviewSizes.ToList();
        }

        public void SetResolution(Size resolution)
        {
            DisconnectCamera();
            MMaxHeight = (int)resolution.Height;
            MMaxWidth = (int)resolution.Width;
            ConnectCamera(Width, Height);
        }

        public void SetOrientation(int degrees)
        {
            DisconnectCamera();
            MCamera.SetDisplayOrientation(90);
            ConnectCamera(Width, Height);
        }

        public Size GetResolution()
        {
            return MCamera.GetParameters().PreviewSize;
        }

        public void TakePicture(string fileName)
        {
            Log.Info(TAG, "Taking picture");
            this.mPictureFileName = fileName;
            // Postview and jpeg are sent in the same buffers if the queue is not empty when performing a capture.
            // Clear up buffers to avoid mCamera.takePicture to be stuck because of a memory issue
            MCamera.SetPreviewCallback(null);

            // PictureCallback is implemented by the current class
            MCamera.TakePicture(null, null, this);
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            Log.Info(TAG, "Saving a bitmap to file");
            // The camera preview was automatically stopped. Start it again.
            MCamera.StartPreview();
            MCamera.SetPreviewCallback(this);

            // Write the image in a file (in jpeg format)
            try
            {
                FileOutputStream fos = new FileOutputStream(mPictureFileName);

                fos.Write(data);
                fos.Close();

            }
            catch (Exception ex)
            {
                Log.Error("PictureDemo", "Exception in photoCallback", ex);
            }

        }
    }
}