using HMNGasApp.Model;
using HMNGasApp.Services;
using HMNGasApp.WebServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HMNGasApp.Tests.Services
{
    public class CustomerSoapServiceTests
    {
        [Fact]
        public async Task GetCustomer_given_valid_login_returns_customer()
        {
            //Arrange
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var customers = new List<Customer>();
            var customer = new Customer()
            {
                AccountNum = "73",
                Address = "Bow St, Smithfield Village, Ireland",
                Email = "test@test.dk",
                Name = "James On",
                Phone = "12345678"
            };

            //Act
            customers.Add(customer);

            var response = new CustomerResponse() { Customers = customers.ToArray() };

            client.Setup(s => s.getCustomers(It.IsAny<CustomerRequest>())).Returns(response);

            var api = new CustomerSoapService(client.Object, config.Object);

            var result = await api.GetCustomer();

            //Assert
            Assert.Equal("73", result.Item2.AccountNum);
            Assert.Equal("Bow St, Smithfield Village, Ireland", result.Item2.Address);
            Assert.Equal("test@test.dk", result.Item2.Email);
            Assert.Equal("James On", result.Item2.Name);
            Assert.Equal("12345678", result.Item2.Phone);
        }

        [Fact]
        public async Task GetCustomer_returns_false_when_api_doesnt_respond()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var service = new CustomerSoapService(client.Object, config.Object);

            var result = await service.GetCustomer();

            Assert.False(result.Item1);
            Assert.Null(result.Item2);
        }

        [Fact]
        public async Task EditCustomer_given_valid_customer_returns_true()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            client.Setup(s => s.newCustContactInfo(It.IsAny<NewCustContactInfoRequest>())).Returns(new NewCustContactInfoResponse { ResponseCode = "Ok", ErrorCode = "0", ResponseMessage = "Succesfuld opdatering af kundedetaljer"});
            var service = new CustomerSoapService(client.Object, config.Object);

            var customer = new Customer();

            var result = await service.EditCustomerAsync(customer);

            Assert.True(result);
        }

        [Fact]
        public async Task EditCustomer_given_invalid_customer_returns_false()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();
            var service = new CustomerSoapService(client.Object, config.Object);

            var customer = new Customer();

            var result = await service.EditCustomerAsync(customer);

            Assert.False(result);
        }
    }
}
