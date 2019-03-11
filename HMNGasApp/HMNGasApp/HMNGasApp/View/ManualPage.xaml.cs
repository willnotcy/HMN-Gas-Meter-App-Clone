using HMNGasApp.ViewModel;

using Xamarin.Forms;

namespace HMNGasApp.View
{
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
