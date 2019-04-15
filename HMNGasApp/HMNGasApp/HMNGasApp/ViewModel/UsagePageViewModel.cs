using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMNGasApp.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Forms;
using System.Linq;

namespace HMNGasApp.ViewModel
{
    class UsagePageViewModel : BaseViewModel
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
            var readings = _config.MeterReadings;
            var plotModel = new PlotModel
            {
                Title = "Seneste forbrug",
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView,
                PlotAreaBorderColor = OxyColors.Transparent
            };

            //TODO Hardcoded color, since impossible to get the correct color from Resource Dictionary ¯\_(ツ)_/¯
            var green = OxyColor.FromRgb(51, 134, 113);

            var series = new BarSeries
            {
                StrokeThickness = 1.0
            };

            readings.Reverse();
            var dates = new List<string>();
            for (int i = 0; i < readings.Count; i++)// var r in readings)
            {
                var r = readings.ElementAt(i);
                var parsedReading = double.Parse(r.Consumption);
                series.Items.Add(new BarItem { Value = parsedReading, Color = green,  });
                dates.Add(r.ReadingDate);
            }

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Top,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Title = "Forbrug (m\u00b3)",
                AxislineStyle = LineStyle.Solid
            });
            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                StringFormat = "yyyy-MM-dd",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Title = "Dato",
                AxislineStyle = LineStyle.Solid,
                ItemsSource = dates
            });
            plotModel.Series.Add(series);

            GraphData = plotModel;
        }
    }
}
