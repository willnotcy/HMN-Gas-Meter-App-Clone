using System;
using System.Windows.Input;
using Xamarin.Forms;
using HMNGasApp.View;

namespace HMNGasApp.ViewModel
{

    public class ManualPageViewModel : BaseViewModel
    {

        public ICommand ManualCommand { get; set; }
        public ICommand BackToHomePageCommand { get; set; }

        public ManualPageViewModel()
        {
            ManualCommand = new Command(async () => await Navigation.PushAsync(new MainPage()));

            BackToHomePageCommand = new Command(() => BackToHomePage());

        }

        async void BackToHomePage()
        {
            await Navigation.PopModalAsync();

        }
      
    }
}
