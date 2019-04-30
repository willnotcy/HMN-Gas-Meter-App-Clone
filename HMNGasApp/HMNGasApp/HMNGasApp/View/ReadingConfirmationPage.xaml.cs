using HMNGasApp.ViewModel;
using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms;

namespace HMNGasApp.View
{
    [ExcludeFromCodeCoverage]
    public partial class ReadingConfirmationPage : ContentPage
    {
        private readonly ReadingConfirmationPageViewModel _vm;

        public ReadingConfirmationPage(string reading)
        {
            InitializeComponent();
            BindingContext = _vm = DependencyService.Resolve<ReadingConfirmationPageViewModel>();
            _vm.Navigation = Navigation;
            _vm.Init(reading);
        }
    }
}
