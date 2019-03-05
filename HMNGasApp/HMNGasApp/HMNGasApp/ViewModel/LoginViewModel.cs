using HMNGasApp.Services;
using HMNGasApp.View;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private static ILoginSoapService _customerSoapService;
        public static ILoginSoapService CustomerSoapService
        {
            get
            {
                if (_customerSoapService == null)
                {
                    _customerSoapService = DependencyService.Get<ILoginSoapService>();
                }

                return _customerSoapService;
            }
        }

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



            SignInCommand = new Command(async () => await ExecuteSignInCommand());
        }

        private async Task ExecuteSignInCommand()
        {
            //TODO Implement with api
            //await CustomerSoapService.NewLogin(CustomerId, Password);
            await CustomerSoapService.NewLogin("1000214", "7151");
            SignedIn = true;
            await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
        }

    }
}
