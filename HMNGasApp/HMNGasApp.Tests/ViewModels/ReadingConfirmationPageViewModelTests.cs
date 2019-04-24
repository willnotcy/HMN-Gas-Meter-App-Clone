using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.ViewModel;
using HMNGasApp.WebServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HMNGasApp.Tests.ViewModels
{
    public class ReadingConfirmationPageViewModelTests
    {
        [Fact]
        public void Model_given_inputs_returns_correct_output()
        {
            //Arrange
            var client = new Mock<IXellentAPI>().Object;
            var config = new Config();
            var service = new MeterReadingSoapService(client, config);

            var readingConfimrationPageViewModel = new ReadingConfirmationPageViewModel(service, config);

            var usageInput = "9999";
            var accountNum = "1234";

            //Act
            readingConfimrationPageViewModel.UsageInput = usageInput;
            readingConfimrationPageViewModel.AccountNum = accountNum;

            //Assert
            Assert.Equal(usageInput, readingConfimrationPageViewModel.UsageInput);
            Assert.Equal(accountNum, readingConfimrationPageViewModel.AccountNum);
        }
    }
}
