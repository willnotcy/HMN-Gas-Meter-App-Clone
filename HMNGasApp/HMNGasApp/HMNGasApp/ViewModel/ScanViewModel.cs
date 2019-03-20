using HMNGasApp.Helpers;
using HMNGasApp.Services;
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

        public string LabelText { get; set; }

        public ScanViewModel()
        {
            _tesseractApi = DependencyService.Get<ITesseract>().TesseractApi;
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

        public async Task Recognise(Stream result)
        {
            if (result == null) return;
            LabelText = "initializing";

            if (!_tesseractApi.Initialized)
            {
                var initialised = await _tesseractApi.Init("any");
                if (!initialised)
                {
                    LabelText = "Couldn't start Tesseract";
                }
                else
                {
                    LabelText = "Processing image...";

                    //TODO Experiment with PageSegmentationMode https://github.com/tesseract-ocr/tesseract/wiki/ImproveQuality#page-segmentation-method
                    _tesseractApi.SetPageSegmentationMode((PageSegmentationMode)5);
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
                        LabelText = textResult;
                    }
                    else
                    {
                        LabelText = "didnt work";
                    }
                }
            }
        }
    }
}
