using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.View;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ILoginSoapService _service;

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

        public LoginViewModel()
        {
            Title = "Log in";

            Password = "";
            CustomerId = "";

            _service = DependencyService.Get<ILoginSoapService>();

            SignInCommand = new Command(async () => await ExecuteSignInCommand());
        }

        private async Task ExecuteSignInCommand()
        {
            var result = await _service.NewLogin(CustomerId, Password);
            if(result.Item1)
            {
                SignedIn = true;
                await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
            } else
            {
                await App.Current.MainPage.DisplayAlert("Fejl", result.Item2, "Okay");
            }
        }
    }
}
