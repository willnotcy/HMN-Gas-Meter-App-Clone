using HMNGasApp.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand SignInCommand { get; set; }

        private bool _signedIn;

        public bool SignedIn
        {
            get => _signedIn;
            set => SetProperty(ref _signedIn, value);
        }

        private int? _customerId;

        public int? CustomerId
        {
            get => _customerId;
            set => SetProperty(ref _customerId, value);
        }


        public LoginViewModel()
        {
            Title = "Log in";
            
            SignInCommand = new Command(async () => await ExecuteSignInCommand());
        }

        private async Task ExecuteSignInCommand()
        {
            //TODO Implement with api
            SignedIn = true;
            await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
        }

    }
}
