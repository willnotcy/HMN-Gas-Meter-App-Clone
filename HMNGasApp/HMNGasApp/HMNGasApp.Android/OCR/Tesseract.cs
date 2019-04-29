using HMNGasApp.Services;
using Tesseract;
using Tesseract.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(HMNGasApp.Droid.Tesseract))]
namespace HMNGasApp.Droid
{
    /// <summary>
    /// Class for retrieving androind specific TesseractApi
    /// </summary>
    public class Tesseract : ITesseract
    {
        public ITesseractApi TesseractApi { get; private set; }


        public Tesseract()
        {
            TesseractApi = new TesseractApi(Android.App.Application.Context, AssetsDeployment.OncePerInitialization);
        }
    }
}