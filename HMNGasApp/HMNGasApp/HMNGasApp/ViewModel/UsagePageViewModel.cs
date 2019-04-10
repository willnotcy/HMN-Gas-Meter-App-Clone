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

            foreach (var r in readings)
            {
					var parsedReading = FormatValueLabel(double.Parse(r.Consumption));
                    var dateTime = Convert.ToDateTime(r.ReadingDate);
                    series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime), parsedReading));
            }

			
           plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left,
											   IsPanEnabled = false,
											   IsZoomEnabled =false
			});
            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom,
				                                  StringFormat = "yyyy-MM-dd",
                                                  IsPanEnabled = false,
                                                  IsZoomEnabled = false
			});
            plotModel.Series.Add(series);

            GraphData = plotModel;
		}

        private double FormatValueLabel(double vl)
        {
            return ((Int64) vl);
        }
	}
}
