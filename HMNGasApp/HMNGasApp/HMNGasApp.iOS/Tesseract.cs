using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using HMNGasApp.Services;
using Tesseract;
using Tesseract.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(HMNGasApp.iOS.Tesseract))]
namespace HMNGasApp.iOS
{
    /// <summary>
    /// Class for retrieving iOS specific TesseractApi
    /// </summary>
    //TODO Check if this dependency works
    public class Tesseract : ITesseract
    {
        public ITesseractApi TesseractApi { get; private set; }

        public Tesseract()
        {
            TesseractApi = new TesseractApi();
        }
    }
}