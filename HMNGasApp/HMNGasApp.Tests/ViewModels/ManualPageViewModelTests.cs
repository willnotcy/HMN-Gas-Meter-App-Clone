using HMNGasApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HMNGasApp.Tests.ViewModels
{
    public class ManualPageViewModelTests
    {
        [Fact]
        public void UsageInput_given_input_can_return_correct_output()
        {
            //Arrange
            var input = "1235678";
            var manualPageViewModel = new ManualPageViewModel();

            //Act
            manualPageViewModel.UsageInput = input;

            var result = manualPageViewModel.UsageInput;

            //Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void Reset_work_correctly()
        {
            //Arrange
            var input = "1235678";
            var manualPageViewModel = new ManualPageViewModel();

            //Act
            manualPageViewModel.UsageInput = input;

            manualPageViewModel.Reset();

            var result = manualPageViewModel.UsageInput;

            //Assert
            Assert.Null(result);
        }
    }
}
