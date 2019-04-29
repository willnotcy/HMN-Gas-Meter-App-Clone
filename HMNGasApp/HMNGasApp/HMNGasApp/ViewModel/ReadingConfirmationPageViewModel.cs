using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HMNGasApp.Model;
using HMNGasApp.Services;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class ReadingConfirmationPageViewModel : BaseViewModel
    {
        private readonly IMeterReadingSoapService _service;
        //Get resources
        private readonly ResourceDictionary res = App.Current.Resources;

        public ICommand ManualCommand { get; set; }
        private readonly IConfig _config;
        public ICommand ReturnNavCommand { get; set; }
        
        private string _usageInput;
        public string UsageInput
        {
            get => _usageInput;
            set => SetProperty(ref _usageInput, value);
        }

        private string _accountNum;
        public string AccountNum
        {
            get => _accountNum;
            set => SetProperty(ref _accountNum, value);
        }

        public ReadingConfirmationPageViewModel(IMeterReadingSoapService service, IConfig config)
        {
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            ManualCommand = new Command(async () => await ExecuteManualCommand());
            _service = service;
            _config = config;
        }

        public void Init(string reading)
        {
            var numberSize = _config.MeterReadings.Count > 0 ? Int32.Parse(_config.MeterReadings[0].NumberSize) : 5;

            if (reading.Contains(".")) 
            {
                reading = reading.Split('.')[0]; 
            }

            if (reading.Length > numberSize)
            {
                reading = reading.Substring(0, numberSize);
            }

            UsageInput = reading;

            AccountNum = _config.CustomerId;

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

        private async Task ExecuteManualCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var result = await _service.NewMeterReadingAsync(UsageInput);

            if (!result.Item1)
            {
                await App.Current.MainPage.DisplayAlert((String)res["Errors.Title.Fail"], result.Item2, (String)res["Errors.Cancel.Okay"]);
                await Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert((String)res["Success.Title.MeterRead"], (String)res["Success.Message.ReadingSent"], (String)res["Success.Cancel.Okay"]);

                this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                await Navigation.PopAsync();
            }
            IsBusy = false;
        }
    }
}

