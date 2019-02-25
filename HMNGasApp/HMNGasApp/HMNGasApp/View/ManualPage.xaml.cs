using System;
using System.Collections.Generic;
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
            viewModel.Navigation = this.Navigation;
        }
    }
}
