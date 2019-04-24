﻿using System;
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
using Plugin.Permissions;
using Plugin.CurrentActivity;
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

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var container = TinyIoCContainer.Current;
            container.Register<IDevice>(AndroidDevice.CurrentDevice);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Resolver.SetResolver(new TinyResolver(container));

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);


            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);

            LoadApplication(new App());
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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