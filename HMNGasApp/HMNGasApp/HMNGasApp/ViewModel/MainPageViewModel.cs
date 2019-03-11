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
        public ICommand LogOutCommand { get; set; }

        public MainPageViewModel()
        {
            ManualPageNavCommand = new Command(async () => await ExecuteManualPageNavCommand());
            InfoPageNavCommand = new Command(async () => await ExecuteInfoPageNavCommand());
            LogOutCommand = new Command(async () => await ExecuteLogOutCommand());
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

            await Navigation.PushAsync(new ManualPage());

            IsBusy = false;
        }
    }
}
