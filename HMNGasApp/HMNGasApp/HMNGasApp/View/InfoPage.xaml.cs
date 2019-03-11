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
            MessagingCenter.Subscribe<InfoViewModel>(this, "EnableEdit", (sender) => ToggleEdit());
        }

        private void ToggleEdit()
        {
            EditableName.IsReadOnly = !EditableName.IsReadOnly;
            EditablePhone.IsReadOnly = !EditablePhone.IsReadOnly;
            EditableEMail.IsReadOnly = !EditableEMail.IsReadOnly;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadCommand.Execute(null);
        }
    }
}