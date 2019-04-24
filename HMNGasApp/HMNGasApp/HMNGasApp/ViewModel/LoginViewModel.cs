using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.View;
using HMNGasApp.WebServices;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ILoginSoapService _service;
        private readonly IConfig _config;
        //Get resources
        private readonly ResourceDictionary res = App.Current.Resources;

        public ICommand SignInCommand { get; set; }

        private bool _signedIn;
        public bool SignedIn
        {
            get => _signedIn;
            set => SetProperty(ref _signedIn, value);
        }

        private string _customerId;
        public string CustomerId
        {
            get => _customerId;
            set => SetProperty(ref _customerId, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel(ILoginSoapService service, IConfig config)
        {
            Title = "Log in";

            _service = service;
            _config = config;

            Password = "";
            CustomerId = "";

            SignInCommand = new Command(async () => await ExecuteSignInCommand());
        }

        private async Task ExecuteSignInCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            var result = await _service.NewLoginAsync(CustomerId, Password);
            if(result.Item1)
            {
                SignedIn = true;
                _config.Context.securityKey = result.Item2;
                await Navigation.PushAsync(new MainPage());
            } else
            {
                await App.Current.MainPage.DisplayAlert((String)res["Errors.Title.Fail"], result.Item2, (String)res["Errors.Cancel.Okay"]);
            }

            IsBusy = false;
        }
    }
}
