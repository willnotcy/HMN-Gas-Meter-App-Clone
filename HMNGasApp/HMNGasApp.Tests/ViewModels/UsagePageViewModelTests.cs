using System;
using System.Collections.Generic;
using System.Text;
using HMNGasApp.Model;
using HMNGasApp.ViewModel;
using HMNGasApp.WebServices;
using Xunit;

namespace HMNGasApp.Tests.ViewModels
{
    public class UsagePageViewModelTests
    {
        [Fact]
        public void Model_given_inpout_creates_some_output()
        {
            //Arrange
            var config = new Config();
            var meterReading = new MeterReading
            {
                ReasonToReading = "Ordinær",
                Reading = "9999",
            };
            config.MeterReadings.Add(meterReading);
            var usagePageViewModel = new UsagePageViewModel(config);

            //Act
            usagePageViewModel.Setup();

            var result = usagePageViewModel.GraphData;

            //Assert
            Assert.NotNull(result);
        }
    }
}
