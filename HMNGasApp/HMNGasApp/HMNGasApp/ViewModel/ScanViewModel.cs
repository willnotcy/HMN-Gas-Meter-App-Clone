using HMNGasApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tesseract;
using Xamarin.Forms;
using XLabs.Ioc;

namespace HMNGasApp.ViewModel
{
    public class ScanViewModel : BaseViewModel
    {
        private readonly ITesseractApi _tesseractApi;


        public ICommand ScanCommand { get; set; }
        public ICommand ReturnNavCommand { get; }

        public ScanViewModel()
        {
            ScanCommand = new Command(async () => await ExecuteScanCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
        }

        private async Task ExecuteScanCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PopModalAsync();

            IsBusy = false;
        }

        private async Task ExecuteReturnNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PopModalAsync();

            IsBusy = false;
        }
    }
}
