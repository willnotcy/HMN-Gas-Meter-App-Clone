using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HMNGasApp.Services;
using HMNGasApp.View;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ILoginSoapService _service;

        public ICommand ManualPageNavCommand { get; set; }
        public ICommand InfoPageNavCommand { get; set; }
        public ICommand ScanPageNavCommand { get; }
        public ICommand LogOutCommand { get; set; }
        public ICommand SignOutCommand { get; set; }

        public MainPageViewModel(ILoginSoapService service)
        {
            _service = service;

            ManualPageNavCommand = new Command(async () => await ExecuteManualPageNavCommand());
            InfoPageNavCommand = new Command(async () => await ExecuteInfoPageNavCommand());
            ScanPageNavCommand = new Command(async () => await ExecuteScanPageNavCommand());
            LogOutCommand = new Command(async () => await ExecuteLogOutCommand());
        }

        private async Task ExecuteScanPageNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PushModalAsync(new ScanPage());

            IsBusy = false;
        }

        private async Task ExecuteLogOutCommand()
		{
            SignOutCommand = new Command(async () => await ExecuteSignOutCommand());
        }

        private async Task ExecuteSignOutCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var result = await _service.Logout();
            if (result)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Noget gik galt - prøv igen", "Okay");
            }

            IsBusy = false;
        }

        private async Task ExecuteInfoPageNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PushModalAsync(new InfoPage());

            IsBusy = false;
        }

        private async Task ExecuteManualPageNavCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PushAsync(new ManualPage());

            IsBusy = false;
        }
    }
}
