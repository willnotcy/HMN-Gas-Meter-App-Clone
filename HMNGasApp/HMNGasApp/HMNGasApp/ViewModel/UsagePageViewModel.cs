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
            var plotModel = new PlotModel { Title = "Seneste forbrug",
                                            PlotAreaBorderColor = OxyColors.Transparent
            };

            var green = OxyColor.FromRgb(51, 134, 113);

            var series = new LineSeries { StrokeThickness = 5.0,
                                          MarkerType = MarkerType.Circle,
                                          MarkerSize = 5,
                                          //TODO Hardcoded color, since impossible to get the correct from Resource Dictionary ¯\_(ツ)_/¯
                                          MarkerFill = green,
                                          Color = green
                                        };
            var area = new AreaSeries { Color = series.Color, StrokeThickness = 0};

            foreach (var r in readings)
            {
				var parsedReading = double.Parse(r.Consumption);
                var dateTime = Convert.ToDateTime(r.ReadingDate);
                var dataPoint = new DataPoint(DateTimeAxis.ToDouble(dateTime), parsedReading);
                series.Points.Add(dataPoint);
                area.Points.Add(dataPoint);
            }

			
           plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left,
											   IsPanEnabled = false,
											   IsZoomEnabled =false, 
                                               Title = "Forbrug (m\u00b3)",
                                               AxislineStyle = LineStyle.Solid,
                                               AxislineThickness = 5.0
           });
            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom,
				                                  StringFormat = "yyyy-MM-dd",
                                                  IsPanEnabled = false,
                                                  IsZoomEnabled = false,
                                                  Title = "Dato",
                                                  AxislineStyle = LineStyle.Solid,
                                                  AxislineThickness = 5.0
            });
            plotModel.Series.Add(area);
            plotModel.Series.Add(series);

            GraphData = plotModel;
		}
	}
}
