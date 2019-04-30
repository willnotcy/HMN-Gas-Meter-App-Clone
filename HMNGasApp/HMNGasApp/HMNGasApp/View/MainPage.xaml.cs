using Xamarin.Forms;
using HMNGasApp.ViewModel;

namespace HMNGasApp.View
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = DependencyService.Resolve<MainPageViewModel>();
            viewModel.Navigation = Navigation;
        }
    }
}
