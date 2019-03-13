using System.Threading.Tasks;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;
using Xamarin.Forms;
using Tesseract;
using System;
using System.Drawing;
using System.IO;
using HMNGasApp.ViewModel;
using Xamarin.Forms.Xaml;
using HMNGasApp.Helpers;
using XLabs.Platform.Device;

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private readonly ITesseractApi _tesseractApi;
        private readonly IDevice _device;
        private readonly ScanViewModel _vm;

        public ScanPage()
        {
            InitializeComponent();

            BindingContext = _vm = DependencyService.Resolve<ScanViewModel>();
            _tesseractApi = Resolver.Resolve<ITesseractApi>();
            _device = Resolver.Resolve<IDevice>();
            _vm.Navigation = Navigation;
        }

        private async void GetPhotoButton_OnClicked(object sender, EventArgs e)
        {
            var mediaStorageOptions = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear
            };
            var result = await _device.MediaPicker.TakePhotoAsync(mediaStorageOptions);

            PhotoImage.Source = ImageSource.FromStream(() => { return result.Source; });

            await Recognise(result.Source);
        }

        private async Task<MediaFile> TakePic()
        {
            var mediaStorageOptions = new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Rear
            };
            var mediaFile = await _device.MediaPicker.TakePhotoAsync(mediaStorageOptions);

            return mediaFile;
        }

        //private async void GetPhotoGrayScaleButton_OnClicked(object sender, EventArgs e)
        //{
        //    var result = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());

        //    var grayScale = DependencyService.Get<IImageHandler>().ToGrayScale(result.Source);

        //    PhotoImage.Source = ImageSource.FromStream(() => DependencyService.Get<IImageHandler>().ToGrayScale(result.Source));

        //    await Recognise(grayScale);
        //}

        public async Task Recognise(Stream result) {
            if (result == null) return;
            activityIndicator.IsRunning = true;
            TextLabel.Text = "initializing";

            if (!_tesseractApi.Initialized)
            {
                var initialised = await _tesseractApi.Init("any");
                if (!initialised)
                {
                    TextLabel.Text = "Couldn't start Tesseract";
                    activityIndicator.IsRunning = false;
                } else
                {
                    TextLabel.Text = "Processing image...";

                    //TODO Experiment with PageSegmentationMode https://github.com/tesseract-ocr/tesseract/wiki/ImproveQuality#page-segmentation-method
                    _tesseractApi.SetPageSegmentationMode((PageSegmentationMode) 5);
                    bool success = await _tesseractApi.SetImage(result);
                    activityIndicator.IsRunning = false;
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
                        TextLabel.Text = textResult;
                    }
                    else
                    {
                        TextLabel.Text = "didnt work";
                    }
                }
            }
        }
    }
}