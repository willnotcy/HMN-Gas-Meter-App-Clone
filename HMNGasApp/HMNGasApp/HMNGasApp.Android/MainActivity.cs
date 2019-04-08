using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using HMNGasApp.Services;
using System.Threading.Tasks;

namespace HMNGasApp.Droid
{
    [Activity(Label = "HMNGasApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
            LoadApplication(new App());
        }

        private void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Task.Run(async () =>
            {
                var service = DependencyService.Get<ILoginSoapService>();
                await service.Logout();
            });
        }
    }
}