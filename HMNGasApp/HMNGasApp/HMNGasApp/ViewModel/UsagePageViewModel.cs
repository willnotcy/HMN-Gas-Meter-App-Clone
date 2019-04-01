using HMNGasApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;
using HMNGasApp.Model;
using Xamarin.Forms;
using System.Linq;

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
			Setup();
		}

		public void Setup()
		{
			var readings= _config.MeterReadings;
			var entries = new List<Microcharts.Entry>();

            float previous = default(float);
			foreach (var r in readings)
			{
                if (previous == default(float))
                {
                    previous = float.Parse(r.Reading);
                    continue;
                }

                if(r.ReasonToReading == "Ordinær")
                {
                    var parsedReading = float.Parse(r.Reading);
				    entries.Add(new Microcharts.Entry(parsedReading - previous) { Label = r.ReadingDate,
																			               ValueLabel = FormatValueLabel(float.Parse(r.Reading) - previous),
																			               Color = SKColor.Parse("#54C7A9")
                    });
                    previous = parsedReading;
                }
			}

			GraphData = new LineChart() { Entries = entries,
										  LineSize = 10,
										  LabelTextSize = 30,
										  PointSize = 40,
                                          LineAreaAlpha = 100, 
                                          LineMode = LineMode.Straight
			};
		}

        private string FormatValueLabel(float vl)
        {
            return "" + ((Int32) vl);
            
        }

		public void Testsetup()
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
