using HMNGasApp.ViewModel;
using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms;

namespace HMNGasApp.View
{
    [ExcludeFromCodeCoverage]
    public partial class ManualPage : ContentPage
    {
        private readonly ManualPageViewModel viewModel;
        public ManualPage()
        {
            InitializeComponent();
            BindingContext = viewModel = DependencyService.Resolve<ManualPageViewModel>();
            viewModel.Navigation = Navigation;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.Reset();
        }
    }
}
