using HMNGasApp.Services;
using HMNGasApp.ViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HMNGasApp.Tests.ViewModels
{
    public class MainPageViewModelTests
    {
        
        [Fact]
        public void Model_contructor_is_correct_type()
        {
            //Arrange
            var service = new Mock<ILoginSoapService>().Object;
            var mainPageViewModel = new MainPageViewModel(service);
            //Act
            var result = mainPageViewModel.GetType();
            //Assert
            Assert.Equal(typeof(MainPageViewModel), result);
        }
    }
}
