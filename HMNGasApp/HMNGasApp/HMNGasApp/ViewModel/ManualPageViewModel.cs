using System;
using System.Windows.Input;
using Xamarin.Forms;
using HMNGasApp.View;
using System.Threading.Tasks;

namespace HMNGasApp.ViewModel
{

    public class ManualPageViewModel : BaseViewModel
    {

        public ICommand ManualCommand { get; set; }
        public ICommand ReturnNavCommand { get; set; }

        private string _usageInput;

        public string UsageInput
        {
            get => _usageInput;
            set => SetProperty(ref _usageInput, value);
        }


        public ManualPageViewModel()
        {
            ManualCommand = new Command(async () => await ExecuteManualCommand());
            ReturnNavCommand = new Command(async () => await ExecuteReturnNavCommand());
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

            if (UsageInput == null || UsageInput.Equals(""))
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Input feltet må ikke være tomt!", "OK");
            }
            else
            {
                await Navigation.PushAsync(new ReadingConfirmationPage(UsageInput));
            }
            IsBusy = false;
        }

        public void Reset()
        {
            UsageInput = null;
        }
    }
}
