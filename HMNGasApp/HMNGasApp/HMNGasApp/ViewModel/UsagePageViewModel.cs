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
                    var parsedReading = float.Parse(r.Reading) - previous;
                    CheckValue(parsedReading);
                    var dateTime = Convert.ToDateTime(r.ReadingDate);
                    CheckDate(dateTime);
                    series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime), parsedReading));
                    previous = parsedReading;
                }
            }


            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = MaximumValue + (MaximumValue * 0.1), Minimum = MinimumValue - (MinimumValue * 0.1)});
            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = DateTimeAxis.ToDouble(Earliest), Maximum = DateTimeAxis.ToDouble(Latest), StringFormat = "yyyy-MM-dd" });
            plotModel.Series.Add(series);

            GraphData = plotModel;
		}

        private string FormatValueLabel(float vl)
        {
            return "" + ((Int64) vl);
        }

        private void CheckValue(float value)
        {
            MinimumValue = MinimumValue < value ? MinimumValue : value;
            MaximumValue = MaximumValue > value ? MaximumValue : value;
        }

        private void CheckDate(DateTime date)
        {
            Earliest = Earliest < date ? Earliest : date;
            Latest = Latest > date ? Latest : date;
        }
	}
}
