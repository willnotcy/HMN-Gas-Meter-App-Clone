using HMNGasApp.Helpers;
using HMNGasApp.Services;
using HMNGasApp.View;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tesseract;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

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
            var res = App.Current.Resources;

            if (Reading == null || Reading.Equals(""))
            {
                await App.Current.MainPage.DisplayAlert((string)res["Errors.Title.Fail"], (string)res["Errors.Message.InputEmpty"], (string)res["Errors.Cancel.Okay"]);
            }
            else if (Reading.Contains("?"))
            {
                await App.Current.MainPage.DisplayAlert((string)res["Errors.Title.Fail"], (string)res["Errors.Message.OCR"], (string)res["Errors.Cancel.Okay"]);
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
                    var res = App.Current.Resources;

                    if (status != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                        {
                            await App.Current.MainPage.DisplayAlert((string)res["Permission.Title.Camera"], (string)res["Permission.Message.AppNeedCamera"], (string)res["Permission.Cancel.Okay"]);
                        }

                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Permission.Camera))
                        {
                            status = results[Permission.Camera];
                        }
                    }
                    //TODO: Double check?
                    if (status == PermissionStatus.Granted)
                    {
                        _openCVService.OpenCamera();
                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        await App.Current.MainPage.DisplayAlert((string)res["Permission.Title.Camera"], (string)res["Permission.Message.AppNotPermitted"], (string)res["Permission.Cancel.Okay"]);
                    }
                });
            }
            catch (Exception)
            {
                var res = App.Current.Resources;
                await App.Current.MainPage.DisplayAlert((string)res["Permission.Title.Camera"], (string)res["Permission.Message.AppNotPermitted"], (string)res["Permission.Cancel.Okay"]);
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
            {
                return; 
            }

            if (!_tesseract.Initialized)
            {
                var initialised = await _tesseract.Init("deu");
                _tesseract.SetWhitelist("0123456789");
                _tesseract.SetPageSegmentationMode(PageSegmentationMode.SingleChar);
                if (!initialised)
                {
                    return;
                }       
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
                    textResult += item;
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
