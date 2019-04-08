using System;
using System.Windows.Input;
using HMNGasApp.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Forms;

namespace HMNGasApp.ViewModel
{
    class UsagePageViewModel:BaseViewModel
    {
		public ICommand ReturnNavCommand { get; set; }
		private readonly IConfig _config;

        private PlotModel _graphData;

        public PlotModel GraphData
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
            var plotModel = new PlotModel { Title = "Seneste forbrug"};

            var series = new LineSeries();

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
                    series.Points.Add(new DataPoint(parsedReading - previous, DateTimeAxis.ToDouble(Convert.ToDateTime(r.ReadingDate))));
                    previous = parsedReading;
                }
            }


            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left});
            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom});
            plotModel.Series.Add(series);

            
            GraphData = plotModel;
		}

        private string FormatValueLabel(float vl)
        {
            return "" + ((Int64) vl);
        }
	}
}
