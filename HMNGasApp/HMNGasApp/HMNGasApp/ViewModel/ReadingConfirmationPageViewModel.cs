using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.View;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class ReadingConfirmationPageViewModel : BaseViewModel
    {
        private readonly IMeterReadingSoapService _service;

        private string _usageInput;
        private string _accountNum;
        public ICommand ManualCommand { get; set; }
        private readonly IConfig _config;

        public ICommand ReturnNavCommand { get; set; }

        public string UsageInput
        {
            get => _usageInput;
            set => SetProperty(ref _usageInput, value);
        }

        public string AccountNum
        {
            get => _accountNum;
            set => SetProperty(ref _accountNum, value);
        }

        public ReadingConfirmationPageViewModel(IMeterReadingSoapService service, IConfig config)
        {
            _service = service;
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            ManualCommand = new Command(async () => await ExecuteManualCommand());
            _config = config;
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
                await App.Current.MainPage.DisplayAlert("Fejl", result.Item2, "OK");
                await Navigation.PopAsync();
            } else
            {
                await App.Current.MainPage.DisplayAlert("Måler aflæst", "Din aflæsning er indsendt.", "OK");
                this.Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                await Navigation.PopAsync();
            }

            IsBusy = false;
        }
    }
}

