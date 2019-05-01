using Android.Content;
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
                    //TODO: Nice case
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