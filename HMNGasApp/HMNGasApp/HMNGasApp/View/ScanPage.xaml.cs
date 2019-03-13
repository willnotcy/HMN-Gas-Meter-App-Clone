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

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private readonly IMediaPicker _mediaPicker;
        private readonly ITesseractApi _tesseractApi;
        private readonly ScanViewModel _vm;

        public ScanPage()
        {
            InitializeComponent();

            BindingContext = _vm = DependencyService.Resolve<ScanViewModel>();
            _vm.Navigation = Navigation;
        }

        private async void GetPhotoButton_OnClicked(object sender, EventArgs e)
        {
            var result = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());

            PhotoImage.Source = ImageSource.FromStream(() => { return result.Source; });

            await Recognise(result.Source);
        }

        private async void GetPhotoGrayScaleButton_OnClicked(object sender, EventArgs e)
        {
            var result = await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions());

            var grayScale = DependencyService.Get<IImageHandler>().ToGrayScale(result.Source);

            PhotoImage.Source = ImageSource.FromStream(() => DependencyService.Get<IImageHandler>().ToGrayScale(result.Source));

            await Recognise(grayScale);
        }

        public async Task Recognise(Stream result) {
            if (result == null) return;
            activityIndicator.IsRunning = true;
            TextLabel.Text = "initializing";
            if (!_tesseractApi.Initialized)
            {
                var initialised = await _tesseractApi.Init("eng");
                if (!initialised)
                    return;
            }
            TextLabel.Text = "processing image...";

            var text = await _vm.Recognise(result);
            activityIndicator.IsRunning = false;

            TextLabel.Text = text;
        }
    }
}