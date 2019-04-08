using HMNGasApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HMNGasApp.Model;
using Xamarin.Forms;
using System.Linq;

namespace HMNGasApp.ViewModel
{
    class UsagePageViewModel:BaseViewModel
    {
		public ICommand ReturnNavCommand { get; set; }
		private readonly IConfig _config;

		public UsagePageViewModel(IConfig config)
		{
			_config = config;
			ReturnNavCommand = new Command(async () => await Navigation.PopModalAsync());
			Setup();
		}

		private void Setup()
		{
			var readings= _config.MeterReadings;
			

		}

        private string FormatValueLabel(float vl)
        {
            return "" + ((Int32) vl);
            
        }
	}
}
