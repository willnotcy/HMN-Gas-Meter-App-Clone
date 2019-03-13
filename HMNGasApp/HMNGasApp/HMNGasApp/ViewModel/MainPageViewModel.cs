using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HMNGasApp.View;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand ManualPageNavCommand { get; set; }
        public ICommand InfoPageNavCommand { get; set; }
        public ICommand ScanPageNavCommand { get; }
        public ICommand LogOutCommand { get; set; }

        public MainPageViewModel()
        {
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
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            await Navigation.PopModalAsync();

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

            await Navigation.PushModalAsync(new ManualPage());

            IsBusy = false;
        }
    }
}
