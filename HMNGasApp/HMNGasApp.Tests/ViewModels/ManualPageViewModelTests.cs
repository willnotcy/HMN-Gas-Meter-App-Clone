using HMNGasApp.Model;
using HMNGasApp.ViewModel;
using Moq;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Text;
=======
>>>>>>> origin/Finish_up
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
            var config = new Mock<IConfig>();
            var manualPageViewModel = new ManualPageViewModel(config.Object)
            {
                UsageInput = input
            };

            //Act
            var result = manualPageViewModel.UsageInput;

            //Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void Reset_work_correctly()
        {
            //Arrange
            var input = "1235678";
            var config = new Mock<IConfig>();
            var manualPageViewModel = new ManualPageViewModel(config.Object)
            {
                UsageInput = input
            };

            //Act

            manualPageViewModel.Reset();

            var result = manualPageViewModel.UsageInput;

            //Assert
            Assert.Null(result);
        }
    }
}
