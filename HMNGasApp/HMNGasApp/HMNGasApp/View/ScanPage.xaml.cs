using Xamarin.Forms;
using HMNGasApp.ViewModel;
using Xamarin.Forms.Xaml;
using System.Diagnostics.CodeAnalysis;

namespace HMNGasApp.View
{
    [ExcludeFromCodeCoverage]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private readonly ScanViewModel _vm;

        public ScanPage()
        {
            InitializeComponent();

            BindingContext = _vm = DependencyService.Resolve<ScanViewModel>();

            _vm.Navigation = Navigation;
            _vm.OpenCameraCommand.Execute(null);
        }
    }
}