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

           // EditableName.TextChanged += OnTextChanged;
           // EditablePhone.TextChanged += OnTextChanged;
           // EditableEMail.TextChanged += OnTextChanged;

            BindingContext = _viewModel = DependencyService.Resolve<InfoViewModel>();
            _viewModel.Navigation = Navigation;
            MessagingCenter.Subscribe<InfoViewModel>(this, "EnableEdit", (sender) => ToggleEdit());
        }

        private void ToggleEdit()
        {
            EditableName.IsReadOnly = !EditableName.IsReadOnly;
            EditablePhone.IsReadOnly = !EditablePhone.IsReadOnly;
            EditableEMail.IsReadOnly = !EditableEMail.IsReadOnly;
            SaveInfoButton.IsVisible = true;
        }
      
       /* private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
                Button.IsVisible = true;
        }*/

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadCommand.Execute(null);
        }
    }
}