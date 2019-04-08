using HMNGasApp.Helpers;
using HMNGasApp.Services;
using HMNGasApp.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tesseract;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace HMNGasApp.ViewModel
{
    /// <summary>
    /// Class containing the view model for the scanpage. This is the class responsible for handling the OCR, by using OpenCV and Tesseract
    /// </summary>
    public class ScanViewModel : BaseViewModel
    {
        private readonly ITesseractApi _tesseractApi;
        private readonly IDevice _device;
        private readonly IOpenCVService _openCVService;
        public ICommand ReturnNavCommand { get; }
        public ICommand OpenCameraCommand { get; }
        public ICommand ConfirmReadingCommand { get; }

        private string _reading;
        public string Reading
        {
            get { return _reading; }
            set { SetProperty(ref _reading, value); }
        }

        private ImageSource _photoImage;
        public ImageSource ImageSource
        {
            get { return _photoImage; }
            set { SetProperty(ref _photoImage, value); }
        }

        public ScanViewModel()
        {
            _tesseractApi = DependencyService.Get<ITesseract>().TesseractApi;
            _openCVService = DependencyService.Get<IOpenCVService>();
            _device = Resolver.Resolve<IDevice>();
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            OpenCameraCommand = new Command(async () => await ExecuteOpenCameraCommand());
            ConfirmReadingCommand = new Command(async () => await ExecuteConfirmReadingCommand());
            MessagingCenter.Subscribe<CameraResultMessage>(this, CameraResultMessage.Key, async (sender) => await HandleResult(sender));
        }

        private async Task ExecuteConfirmReadingCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            if (Reading == null || Reading.Equals(""))
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Input feltet må ikke være tomt!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ReadingConfirmationPage(Reading));
            }
            IsBusy = false;
        }

        private async Task HandleResult(CameraResultMessage sender)
        {
            var digits = sender.Digits;
            var image = sender.Image;

            Reading = "";

            foreach (Stream stream in digits)
            {
                await Recognise(stream);
            }

            ImageSource = ImageSource.FromStream(() => { return image; });
        }
        private async Task ExecuteReturnNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PopAsync();

            IsBusy = false;
        }

        public async Task ExecuteOpenCameraCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Task.Run(() =>
            {
                _openCVService.OpenCamera();
            });

            IsBusy = false;
        }
        
        /// <summary>
        /// Method for recognizing the characters. Input comes from the ScanPage.xaml.cs, and is then processed into words
        /// </summary>
        /// <param name="result">Stream of bytes of the picture to process</param>
        /// <returns></returns>
        private async Task Recognise(Stream result)
        {
            if (result == null) return;
            if (!_tesseractApi.Initialized)
            {
                var initialised = await _tesseractApi.Init("any");
                if (!initialised)
                {
                    Reading = "Couldn't start Tesseract";
                }
                else
                {
                    //TODO Experiment with PageSegmentationMode https://github.com/tesseract-ocr/tesseract/wiki/ImproveQuality#page-segmentation-method
                    //_tesseractApi.SetPageSegmentationMode((PageSegmentationMode) 5);

                    //TODO Implement opencv image processing
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
                        Reading = textResult;
                    }
                    else
                    {
                        Reading = "Didn't work";
                    }
                }
            }
        }
    }
}
