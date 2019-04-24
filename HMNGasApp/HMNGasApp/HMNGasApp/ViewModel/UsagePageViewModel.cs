using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMNGasApp.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Forms;
using System.Linq;
using System.Globalization;

namespace HMNGasApp.ViewModel
{
    public class UsagePageViewModel : BaseViewModel
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
            ReturnNavCommand = new Command(async () => await Navigation.PopAsync());
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

            var series = new ColumnSeries
            {
                StrokeThickness = 1.0
            };

            var dates = new List<string>();
            for (int i = readings.Count - 5; i < readings.Count; i++)
            {
                var r = readings.ElementAt(i);
                var parsedReading = double.Parse(r.Consumption, new CultureInfo("da"));
                series.Items.Add(new ColumnItem { Value = parsedReading, Color = green  });
                dates.Add(FormatDate(r.ReadingDate));
            }

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Title = "Forbrug (m\u00b3)",
                AxislineStyle = LineStyle.Solid
            });
            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd-MM-yy",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Title = "Dato",
                AxislineStyle = LineStyle.Solid,
                ItemsSource = dates
            });
            plotModel.Series.Add(series);

            GraphData = plotModel;
        }

        private string FormatDate(string input)
        {
            var dateTime = Convert.ToDateTime(input);

            return dateTime.ToString("dd-MM-yy");
        }
    }
}
