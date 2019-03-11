using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HMNGasApp.Model;
using HMNGasApp.View;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class ReadingConfirmationPageViewModel : BaseViewModel
    {
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

        public ReadingConfirmationPageViewModel(IConfig config)
        {
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
            ManualCommand = new Command(async () => await ExecuteManualCommand());
            _config = config;
            Init();

        }

        private void Init()
        {
            AccountNum = _config.CustomerId;
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

        private async Task ExecuteManualCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await App.Current.MainPage.DisplayAlert("Måler aflæst", "Din aflæsning er indsendt.", "OK");
            this.Navigation.RemovePage(Navigation.ModalStack[Navigation.ModalStack.Count - 2]);
            await Navigation.PopModalAsync();

            IsBusy = false;
        }
    }
}

