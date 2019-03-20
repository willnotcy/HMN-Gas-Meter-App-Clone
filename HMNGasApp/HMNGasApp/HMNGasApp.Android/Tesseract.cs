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
using Tesseract;
using Tesseract.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(HMNGasApp.Droid.Tesseract))]
namespace HMNGasApp.Droid
{
    public class Tesseract : ITesseract
    {
        public ITesseractApi TesseractApi { get; private set; }


        public Tesseract()
        {
            TesseractApi = new TesseractApi(Android.App.Application.Context, AssetsDeployment.OncePerInitialization);
        }
    }
}