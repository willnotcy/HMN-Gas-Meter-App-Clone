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
        public ICommand ReturnNavCommand { get; }
        public ICommand TakePictureCommand { get; }

        private string _labelText;
        public string LabelText
        {
            get { return _labelText; }
            set { SetProperty(ref _labelText, value); }
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
            _device = Resolver.Resolve<IDevice>();
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            TakePictureCommand = new Command(async () => await ExecuteTakePictureCommand());
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

        public async Task ExecuteTakePictureCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var mediaStorageOptions = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear
            };
            var result = await _device.MediaPicker.TakePhotoAsync(mediaStorageOptions);

            ImageSource = ImageSource.FromStream(() => { return result.Source; });

            await Recognise(result.Source);

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
            LabelText = "Initializing";

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
                    _tesseractApi.SetPageSegmentationMode((PageSegmentationMode) 5);

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
                        LabelText = textResult;
                    }
                    else
                    {
                        LabelText = "Didn't work";
                    }
                }
            }
        }
    }
}
