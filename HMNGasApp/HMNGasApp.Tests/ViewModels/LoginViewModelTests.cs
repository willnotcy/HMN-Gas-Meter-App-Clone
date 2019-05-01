using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.ViewModel;
using HMNGasApp.WebServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xunit;

namespace HMNGasApp.Tests.ViewModels
{
    public class LoginViewModelTests
    {
        [Fact]
        public void Model_given_inputs__can_return_inputs()
        {
            //Arrange
            var client = new Mock<IXellentAPI>();
            var connectService = new Mock<IConnectService>();
            var config = new Config();
            var meterReadingService = new Mock<IMeterReadingSoapService>();
            var service = new LoginSoapService(client.Object, connectService.Object, config, meterReadingService.Object);

            var loginViewModel = new LoginViewModel(service, config);

            var isBusy = false;
            var navigation = new Mock<INavigation>().Object;
            var password = "password";
            var signedIn = true;
            var title = "titleTest";
            var customerId = "1234";

            //Act
            loginViewModel.IsBusy = isBusy;
            loginViewModel.Navigation = navigation;
            loginViewModel.Password = password;
            loginViewModel.SignedIn = signedIn;
            loginViewModel.Title = title;
            loginViewModel.CustomerId = customerId;

            //Assert
            Assert.Equal(loginViewModel.IsBusy, isBusy);
            Assert.Equal(loginViewModel.Navigation, navigation);
            Assert.Equal(loginViewModel.Password, password);
            Assert.Equal(loginViewModel.SignedIn, signedIn);
            Assert.Equal(loginViewModel.Title, title);
            Assert.Equal(loginViewModel.CustomerId, customerId);
        }
    }
}
