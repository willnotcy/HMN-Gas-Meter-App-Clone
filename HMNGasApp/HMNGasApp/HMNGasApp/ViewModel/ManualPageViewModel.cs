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
        public ICommand SettingsPageNavCommand { get; set; }

        public ManualPageViewModel()
        {
            ManualCommand = new Command(async () => await ExecuteManualCommand());
            ReturnNavCommand = new Command(async () => await Navigation.PopModalAsync());
            //SettingsPageNavCommand = new Command(async () => await Navigation.PushModalAsync(new SettingsPage()));

        }

        private async Task ExecuteManualCommand()
        {
            await App.Current.MainPage.DisplayAlert("Data indsendt", "Din manuelle indtastning er blevet godkendt", "Tilbage til menu");
            await Navigation.PushModalAsync(new MainPage());
        }

    }
}
