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
            var color = (Color) Application.Current.Resources["PrimaryOrange"];
            EditableName.IsReadOnly = !EditableName.IsReadOnly;
            EditableName.TextColor = color;
            EditablePhone.IsReadOnly = !EditablePhone.IsReadOnly;
            EditablePhone.TextColor = color;
            EditableEMail.IsReadOnly = !EditableEMail.IsReadOnly;
            EditableEMail.TextColor = color;
            SaveInfoButton.IsVisible = true;
        }
      
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadCommand.Execute(null);
        }
    }
}