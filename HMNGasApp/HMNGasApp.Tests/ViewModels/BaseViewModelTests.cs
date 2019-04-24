using HMNGasApp.ViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xunit;

namespace HMNGasApp.Tests.ViewModels
{
    public class BaseViewModelTests
    {
        [Fact]
        public void IsBusy_given_true_is_True()
        {
            //Arrange
            var baseViewModel = new BaseViewModel();

            //Act
            baseViewModel.IsBusy = true;
            var result = baseViewModel.IsBusy;
            
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void IsBusy_given_false_is_false()
        {
            //Arrange
            var baseViewModel = new BaseViewModel();
            
            //Act
            baseViewModel.IsBusy = false;
            var result = baseViewModel.IsBusy;
            
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Title_given_input_returns_input()
        {
            //Arrange
            var input = "myTitle";
            var baseViewModel = new BaseViewModel();

            //Act
            baseViewModel.Title = input;
            var result = baseViewModel.Title;
            
            //Assert
            Assert.Equal(input,result);
        }
        [Fact]
        public void INavigationTEST()
        {
            //Arrange
            var input = new Mock<INavigation>();
            var baseViewModel = new BaseViewModel();

            //Act
            baseViewModel.Navigation = input.Object;
            var result = baseViewModel.Navigation;

            //Assert
            Assert.Equal(input.Object, result);
        }
    }
}
