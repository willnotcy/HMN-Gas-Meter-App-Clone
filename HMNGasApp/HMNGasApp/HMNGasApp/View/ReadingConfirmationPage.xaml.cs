using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMNGasApp.ViewModel;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace HMNGasApp.View
{
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
        public ReadingConfirmationPage() 
        {
            InitializeComponent();
        }

    }
}
