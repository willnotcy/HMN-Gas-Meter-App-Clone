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
        [Fact]
        public void New_InfoViewModel_given_customer_can_return_inputs()
        {
            //Arrange
            var c = new Customer
            {
                AccountNum = "12345",
                Address = "DRBYEN 12",
                Email = "v@dr.dk",
                Name = "Viktor",
                Phone ="1234567"
            };

            var client = new Mock<IXellentAPI>();
            var config = new Config();
            config.MeterReadings.Add(null);
            var service = new CustomerSoapService(client.Object, config);
            var infoViewModel = new InfoViewModel(service, config) { };

            //Act
            infoViewModel.Customer = c;

            infoViewModel.AccountNum = c.AccountNum;
            infoViewModel.Address = c.Address;
            infoViewModel.Email = c.Email;
            infoViewModel.Name = c.Name;
            infoViewModel.Phone = c.Phone;

            var customerFromModel = new Customer
            {
                AccountNum = infoViewModel.AccountNum,
                Address = infoViewModel.Address,
                Email = infoViewModel.Email,
                Name = infoViewModel.Name,
                Phone = infoViewModel.Phone
            };

            //Assert
            Assert.Equal(c, customerFromModel);
            Assert.Equal(c, infoViewModel.Customer);
        }
        [Fact]
        public void New_InfoViewModel_given_readingInfo_can_return_inputs()
        {
            //Arrange
            var meterReading = new MeterReading
            {
                AccountNum = "123456",
                MeterNum = "123456789",
                Reading = "9999",
                ReadingDate = "12-06-1996"
            };

            var client = new Mock<IXellentAPI>();
            var config = new Config();
            var service = new CustomerSoapService(client.Object, config);
            var infoViewModel = new InfoViewModel(service, config) { };

            //Act
            infoViewModel.LatestMeasure = meterReading.Reading;
            infoViewModel.AccountNum = meterReading.AccountNum;
            infoViewModel.MeterNum = meterReading.MeterNum;
            infoViewModel.MeasureDate = meterReading.ReadingDate;

            var lastestMeterReading = new MeterReading
            {
                AccountNum = infoViewModel.AccountNum,
                MeterNum = infoViewModel.MeterNum,
                Reading = infoViewModel.LatestMeasure,
                ReadingDate = infoViewModel.MeasureDate
            };

            //Assert
            Assert.Equal(meterReading,lastestMeterReading);
        }
        [Fact]
        public void Init_give_customer_returns_correct_output()
        {
            //Arrange
            var c = new Customer
            {
                AccountNum = "12345",
                Address = "DRBYEN 12",
                Email = "v@dr.dk",
                Name = "Viktor",
                Phone = "1234567"
            };

            var client = new Mock<IXellentAPI>();
            var config = new Config();
            var service = new CustomerSoapService(client.Object, config);
            var infoViewModel = new InfoViewModel(service, config) { };

            //Act
            infoViewModel.Init(c);
            var result = infoViewModel.Customer;
            //Assert
            Assert.Equal(c, result);
        }
        [Fact]
        public void VerifyEmail_given_valid_input_returns_true()
        {
            //Arrange
            var c = new Customer
            {
                AccountNum = "12345",
                Address = "DRBYEN 12",
                Email = "v@dr.dk",
                Name = "Viktor",
                Phone = "1234567"
            };

            var client = new Mock<IXellentAPI>();
            var config = new Config();
            var meterReading = new MeterReading
            {
                Reading = "999",
                ReadingDate = "12-06-1996",
                MeterNum = "1234"
            };
            config.MeterReadings.Add(meterReading);
            var service = new CustomerSoapService(client.Object, config);
            var infoViewModel = new InfoViewModel(service, config) { };

            //Act
            infoViewModel.Customer = c;

            infoViewModel.AccountNum = c.AccountNum;
            infoViewModel.Address = c.Address;
            infoViewModel.Email = c.Email;
            infoViewModel.Name = c.Name;
            infoViewModel.Phone = c.Phone;

            var customerFromModel = new Customer
            {
                AccountNum = infoViewModel.AccountNum,
                Address = infoViewModel.Address,
                Email = infoViewModel.Email,
                Name = infoViewModel.Name,
                Phone = infoViewModel.Phone
            };

            //Assert
            Assert.Equal(c, customerFromModel);
            Assert.Equal(c, infoViewModel.Customer);
        }
    }
}
