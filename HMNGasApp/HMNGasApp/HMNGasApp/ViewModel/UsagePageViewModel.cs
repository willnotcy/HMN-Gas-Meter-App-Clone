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

        private double MinimumValue;
        private double MaximumValue;
        private DateTime Earliest;
        private DateTime Latest;

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
                if (r.ReasonToReading == "Ordinær")
                {
					var parsedReading = double.Parse(r.Consumption);
					CheckValue(parsedReading);
                    var dateTime = Convert.ToDateTime(r.ReadingDate);
                    CheckDate(dateTime);
                    series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime), parsedReading));
                }
            }

			
           plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left/*,
												Maximum = MaximumValue + (MaximumValue * 0.1),
												Minimum = MinimumValue - (MinimumValue * 0.1),
												IsPanEnabled = false,
												IsZoomEnabled =false,
				//								MajorTickSize = 7*/
			});
            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom,
				StringFormat = "yyyy-MM-dd"/*,
												  Minimum = DateTimeAxis.ToDouble(Earliest),
											  	  Maximum = DateTimeAxis.ToDouble(Latest), 
												  IsPanEnabled = false,
												  IsZoomEnabled = false*/
			});
            plotModel.Series.Add(series);

            GraphData = plotModel;
		}

        private string FormatValueLabel(double vl)
        {
            return "" + ((Int64) vl);

        }

        private void CheckValue(double value)
        {
            MinimumValue = MinimumValue < value && MinimumValue != default(double) ? MinimumValue : value;
            MaximumValue = MaximumValue > value ? MaximumValue : value;
        }

        private void CheckDate(DateTime date)
        {
            Earliest = Earliest < date ? Earliest : date;
            Latest = Latest > date ? Latest : date;

        }
	}
}
