using HMNGasApp.ViewModel;
using System;
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
            EditableName.BackgroundColor = Color.LightBlue;
            EditablePhone.IsReadOnly = !EditablePhone.IsReadOnly;
            EditablePhone.BackgroundColor = Color.LightBlue;
            EditableEMail.IsReadOnly = !EditableEMail.IsReadOnly;
            EditableEMail.BackgroundColor = Color.LightBlue;
            SaveInfoButton.IsVisible = true;
        }
      
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadCommand.Execute(null);
        }
    }
}