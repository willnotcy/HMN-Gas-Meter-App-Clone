using HMNGasApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsagePage : ContentPage
	{
		private readonly UsagePageViewModel viewModel;
		public UsagePage()
		{
			InitializeComponent ();
			BindingContext = viewModel = DependencyService.Resolve<UsagePageViewModel>();
			viewModel.Navigation = Navigation;
		}
	}
}