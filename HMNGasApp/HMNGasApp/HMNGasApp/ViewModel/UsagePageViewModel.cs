using HMNGasApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;
using HMNGasApp.Model;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    class UsagePageViewModel:BaseViewModel
    {
		private Chart _GraphData;

		public Chart GraphData
		{
			get => _GraphData;
			set => SetProperty(ref _GraphData, value);
		}

		public ICommand ReturnNavCommand { get; set; }
		private readonly IConfig _config;
		public UsagePageViewModel(IConfig config)
		{
			_config = config;
			ReturnNavCommand = new Command(async () => await Navigation.PopModalAsync());
			setup();
		}

		public void setup()
		{
			var readings= _config.MeterReadings;
			var entries = new List<Microcharts.Entry>();
			foreach(var r in readings)
			{
				entries.Add(new Microcharts.Entry(float.Parse(r.Reading)) { Label = r.ReadingDate, ValueLabel = r.Reading});
			}
			GraphData = new LineChart() { Entries = entries };
		}

		public void testsetup()
		{
			var entries = new[]{
				 new Microcharts.Entry(212)
				 {
					 Label = "UWP",
					 ValueLabel = "212",
					 Color = SKColor.Parse("#2c3e50")
				 },
				 new Microcharts.Entry(248)
				 {
					 Label = "Android",
					 ValueLabel = "248",
					 Color = SKColor.Parse("#77d065")
				 },
				 new Microcharts.Entry(128)
				 {
					 Label = "iOS",
					 ValueLabel = "128",
					 Color = SKColor.Parse("#b455b6")
				 },
				 new Microcharts.Entry(514)
				 {
					 Label = "Shared",
					 ValueLabel = "514",
					 Color = SKColor.Parse("#3498db")
				 }
			};
			GraphData = new LineChart() { Entries = entries };

		}
	}
}
