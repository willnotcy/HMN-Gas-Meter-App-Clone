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
    public class InfoViewModelTests
    {
        [Theory]
        [InlineData("hej@gmail.dk", true)]
        [InlineData("h12ej@gmail.dk", true)]
        [InlineData("hej@gmail", false)]
        [InlineData("@gmail.dk", false)]
        [InlineData("hejgmail.dk", false)]
        [InlineData("hej@.dk", false)]
        [InlineData("hejm@eddig", false)]
        [InlineData("hejmeddig", false)]
        public void VerifyMailReturnsCorrect(string input, bool expected)
        {
            var cusService = new Mock<ICustomerSoapService>();
            var config = new Mock<IConfig>();
            var vm = new InfoViewModel(cusService.Object, config.Object);

            var res = vm.VerifyEmail(input);

            Assert.Equal(expected, res);
        }
    }
}
