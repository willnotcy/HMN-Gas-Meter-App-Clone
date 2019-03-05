using HMNGasApp.View;
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
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            

            //TODO Implement with api
            SignedIn = true;
            await Navigation.PushModalAsync(new NavigationPage(new MainPage()));

            IsBusy = false;
        }

    }
}
