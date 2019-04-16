using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Tesseract;
using Tesseract.iOS;
using TinyIoC;
using UIKit;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;
using XLabs.Platform.Device;
using HMNGasApp.Services;
using UIKit;
using Xamarin.Forms;

namespace HMNGasApp.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            var container = TinyIoCContainer.Current;

            container.Register<ITesseractApi>((cont, parameters) =>
            {
                return new TesseractApi();
            });
            Resolver.SetResolver(new TinyResolver(container));

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, arg) => {
                Task.Run(async () =>
                {
                    var service = DependencyService.Get<ILoginSoapService>();
                    await service.Logout();
                });
            });
        }
    }
}
