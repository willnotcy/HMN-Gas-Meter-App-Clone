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
            ManualPageNavCommand = new Command(async () => await Navigation.PushModalAsync(new ManualPage()));
            InfoPageNavCommand = new Command(async () => await Navigation.PushModalAsync(new InfoPage()));
            LogOutCommand = new Command(async () => await ExecuteLogOutCommand());
        }

        private async Task ExecuteLogOutCommand()
        {
            await Navigation.PopModalAsync();
        }
    }
}
