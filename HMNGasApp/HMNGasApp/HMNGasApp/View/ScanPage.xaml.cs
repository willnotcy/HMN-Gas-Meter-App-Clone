using System.Threading.Tasks;
using XLabs.Ioc;
using XLabs.Platform.Services.Media;
using Xamarin.Forms;
using System;
using HMNGasApp.ViewModel;
using Xamarin.Forms.Xaml;
using XLabs.Platform.Device;

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private readonly IDevice _device;
        private readonly ScanViewModel _vm;

        public ScanPage()
        {
            InitializeComponent();

            BindingContext = _vm = DependencyService.Resolve<ScanViewModel>();
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

            await _vm.Recognise(result.Source);
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
    }
}