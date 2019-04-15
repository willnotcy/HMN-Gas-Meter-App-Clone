using HMNGasApp.Helpers;
using HMNGasApp.Services;
using HMNGasApp.View;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ITesseractApi _tesseract;
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

        private ImageSource _digitSource2;
        public ImageSource DigitSource2
        {
            get { return _digitSource2; }
            set { SetProperty(ref _digitSource2, value); }
        }

        private ImageSource _digitSource3;
        public ImageSource DigitSource3
        {
            get { return _digitSource3; }
            set { SetProperty(ref _digitSource3, value); }
        }

        private ImageSource _digitSource4;
        public ImageSource DigitSource4
        {
            get { return _digitSource4; }
            set { SetProperty(ref _digitSource4, value); }
        }

        private ImageSource _digitSource5;
        public ImageSource DigitSource5
        {
            get { return _digitSource5; }
            set { SetProperty(ref _digitSource5, value); }
        }

        private ImageSource _digitSource6;
        public ImageSource DigitSource6
        {
            get { return _digitSource6; }
            set { SetProperty(ref _digitSource6, value); }
        }

        private ImageSource _digitSource7;
        public ImageSource DigitSource7
        {
            get { return _digitSource7; }
            set { SetProperty(ref _digitSource7, value); }
        }

        private ImageSource _digitSource8;
        public ImageSource DigitSource8
        {
            get { return _digitSource8; }
            set { SetProperty(ref _digitSource8, value); }
        }

        private ImageSource _digitSource;
        public ImageSource DigitSource
        {
            get { return _digitSource; }
            set { SetProperty(ref _digitSource, value); }
        }

        public ScanViewModel()
        {
            _tesseract = DependencyService.Get<ITesseract>().TesseractApi;
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
            var digitsClone = sender.DigitsClone;

            Reading = "";

            var i = 0;

            foreach (Stream stream in digits)
            {
                switch (i)
                {
                    case 0:
                        DigitSource = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 1:
                        DigitSource2 = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 2:
                        DigitSource3 = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 3:
                        DigitSource4 = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 4:
                        DigitSource5 = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 5:
                        DigitSource6 = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 6:
                        DigitSource7 = ImageSource.FromStream(() => { return stream; });
                        break;
                    case 7:
                        DigitSource8 = ImageSource.FromStream(() => { return stream; });
                        break;
                }


                await Recognise(digitsClone[i]);
                i++;
            }

            Reading = Reading.Replace(" ", "");

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


            try
            {
                await Task.Run(async () =>
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                    if (status != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                        {
                            await App.Current.MainPage.DisplayAlert("Kamera tilladelse", "Appen skal bruge dit kamera til at udføre scanningen", "OK");
                        }

                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Permission.Camera))
                            status = results[Permission.Camera];
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        _openCVService.OpenCamera();
                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        await App.Current.MainPage.DisplayAlert("Kamera tilladelse", "Appen har ikke tilladelse til at bruge dit kamera", "OK");
                    }
                });
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Kamera tilladelse", "Appen har ikke tilladelse til at bruge dit kamera", "OK");
            }
            

            IsBusy = false;
        }
        
        /// <summary>
        /// Method for recognizing the characters. Input comes from the ScanPage.xaml.cs, and is then processed into words
        /// </summary>
        /// <param name="result">Stream of bytes of the picture to process</param>
        /// <returns></returns>
        private async Task Recognise(Stream result)
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            if (result == null)
                return;

            if (!_tesseract.Initialized)
            {
                var initialised = await _tesseract.Init("deu");
                _tesseract.SetWhitelist("0123456789");
                _tesseract.SetPageSegmentationMode(PageSegmentationMode.SingleChar);
                if (!initialised)
                    return;
            }

            
            bool success = await _tesseract.SetImage(result);
            if (success)
            {
                var words = _tesseract.Results(PageIteratorLevel.Word);
                var symbols = _tesseract.Results(PageIteratorLevel.Symbol);
                var blocks = _tesseract.Results(PageIteratorLevel.Block);
                var paragraphs = _tesseract.Results(PageIteratorLevel.Paragraph);
                var lines = _tesseract.Results(PageIteratorLevel.Textline);

                var textResult = "";

                var enumerator = words.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current.Text;
                    textResult += item ;
                }

                bool isDigit = textResult.All(char.IsDigit);

                if (!isDigit)
                    textResult = "?";

                Reading += textResult;
            }
            else
            {
                Reading = "didnt work";
            }

            IsBusy = false;
        }
    }
}
