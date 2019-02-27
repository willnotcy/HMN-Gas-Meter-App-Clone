
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HMNGasApp.ViewModel;

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _vm;

		public LoginPage ()
		{
			InitializeComponent();
            BindingContext = _vm = DependencyService.Resolve<LoginViewModel>();
            _vm.Navigation = Navigation;
		}
	}
}