using HMNGasApp.Model;
using HMNGasApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //TODO: _viewModel.Navigation = Navigation;
            //_viewModel.AccountNum = customer.AccountNum;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //_viewModel.Reset();
        }
    }
}