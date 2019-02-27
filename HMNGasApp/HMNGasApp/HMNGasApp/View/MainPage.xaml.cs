using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using HMNGasApp.ViewModel;
using HMNGasApp.View;

namespace HMNGasApp.View
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = DependencyService.Resolve<MainPageViewModel>();
            viewModel.Navigation = this.Navigation;
        }
    }
}
