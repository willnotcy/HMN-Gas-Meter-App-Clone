using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HMNGasApp.ViewModel;
using System.Diagnostics.CodeAnalysis;

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ExcludeFromCodeCoverage]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _vm;

		public LoginPage()
		{
			InitializeComponent();
            BindingContext = _vm = DependencyService.Resolve<LoginViewModel>();
            _vm.Navigation = Navigation;
		}
	}
}