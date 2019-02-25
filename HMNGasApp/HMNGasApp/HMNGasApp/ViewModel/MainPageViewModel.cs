using System;
using System.Windows.Input;
using HMNGasApp.View;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand ManualNavCommand { get; set; }

        public MainPageViewModel()
        {
            ManualNavCommand = new Command(async () => await Navigation.PushAsync(new View.ManualPage()));
        }
    }
}
