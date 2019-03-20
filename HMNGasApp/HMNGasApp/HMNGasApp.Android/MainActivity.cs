using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Tesseract;
using Tesseract.Droid;
using Android.Content;
using TinyIoC;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;

namespace HMNGasApp.Droid
{
    [Activity(Label = "HMNGasApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var container = TinyIoCContainer.Current;
            container.Register<IDevice>(AndroidDevice.CurrentDevice);

            Resolver.SetResolver(new TinyResolver(container));

            LoadApplication(new App());
        }
    }
}