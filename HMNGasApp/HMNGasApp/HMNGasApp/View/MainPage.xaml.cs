using Xamarin.Forms;
using HMNGasApp.ViewModel;
using System.Diagnostics.CodeAnalysis;

namespace HMNGasApp.View
{
    [ExcludeFromCodeCoverage]
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
