using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;
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