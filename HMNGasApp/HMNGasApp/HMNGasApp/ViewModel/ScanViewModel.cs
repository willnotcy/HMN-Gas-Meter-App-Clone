using HMNGasApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tesseract;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class ScanViewModel : BaseViewModel
    {
        private readonly IImageHandler _handler;
        private readonly ITesseractApi _tesseractApi;


        public ICommand ScanCommand { get; set; }
        public ICommand ReturnNavCommand { get; }

        public ScanViewModel(IImageHandler handler, ITesseractApi tesseractApi)
        {
            _handler = handler;
            _tesseractApi = tesseractApi;

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

        public async Task<string> Recognise(Stream result)
        {
            bool success = await _tesseractApi.SetImage(result);
            if (success)
            {
                var words = _tesseractApi.Results(PageIteratorLevel.Word);
                var symbols = _tesseractApi.Results(PageIteratorLevel.Symbol);
                var blocks = _tesseractApi.Results(PageIteratorLevel.Block);
                var paragraphs = _tesseractApi.Results(PageIteratorLevel.Paragraph);
                var lines = _tesseractApi.Results(PageIteratorLevel.Textline);

                var textResult = "";

                var enumerator = words.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current.Text;
                    textResult += item + " ";
                }
                return textResult;
            }
            else
            {
                return "didnt work";
            }
        }

    }
}
