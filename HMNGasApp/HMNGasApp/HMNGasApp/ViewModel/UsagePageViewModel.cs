using HMNGasApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HMNGasApp.Model;
using Xamarin.Forms;
using System.Linq;
using XLabs.Forms.Charting.Controls;

namespace HMNGasApp.ViewModel
{
    class UsagePageViewModel:BaseViewModel
    {
		public ICommand ReturnNavCommand { get; set; }
		private readonly IConfig _config;

        private SeriesCollection _graphData;

        public SeriesCollection GraphData
        {
            get { return _graphData; }
            set { SetProperty(ref _graphData, value); }
        }

        public UsagePageViewModel(IConfig config)
		{
			_config = config;
			ReturnNavCommand = new Command(async () => await Navigation.PopModalAsync());
			Setup();
		}

		private void Setup()
		{
			var readings= _config.MeterReadings;
            var seriesCollection = new SeriesCollection();
            var series = new Series { Type = ChartType.Line, Color = (Color) App.Current.Resources["PrimaryGreen"]};
            
            
            var entries = new DataPointCollection();

            float previous = default(float);
            foreach (var r in readings)
            {
                if (previous == default(float))
                {
                    previous = float.Parse(r.Reading);
                    continue;
                }

                if (r.ReasonToReading == "Ordinær")
                {
                    var parsedReading = float.Parse(r.Reading);
                    entries.Add(new DataPoint { Value = parsedReading - previous, Label = r.ReadingDate, Color = (Color) App.Current.Resources["PrimaryGreen"]});
                    previous = parsedReading;
                }
            }

            series.Points = entries;
            seriesCollection.Add(series);
            GraphData = seriesCollection;
		}

        private string FormatValueLabel(float vl)
        {
            return "" + ((Int64) vl);
        }
	}
}
