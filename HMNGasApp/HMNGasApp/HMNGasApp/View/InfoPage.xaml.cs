using HMNGasApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HMNGasApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        private readonly InfoViewModel _viewModel;


        public InfoPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = DependencyService.Resolve<InfoViewModel>();
            _viewModel.Navigation = Navigation;
        }
      
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadCommand.Execute(null);
        }
    }
}