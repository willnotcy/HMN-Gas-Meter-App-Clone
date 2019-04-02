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

namespace HMNGasApp.Droid
{
    class Callback : BaseLoaderCallback
    {
        private readonly OpenCVServiceDroid _activity;
        public Callback(Context context, OpenCVServiceDroid activity)
            : base(context)
        {
            _activity = activity;
        }
        public override void OnManagerConnected(int status)
        {
            switch (status)
            {
                case LoaderCallbackInterface.Success:
                    {
                    
                    }
                    break;
                default:
                    {
                        base.OnManagerConnected(status);
                    }
                    break;
            }
        }
    }
}