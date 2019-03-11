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
        public void GetCustomer_returns_customer()
        {
            var client = new Mock<IXellentAPI>();
            var config = new Mock<IConfig>();

            var customers = new List<WebServices.Customer>();
            customers.Add(new WebServices.Customer() { AccountNum = "73" , Address = "Bow St, Smithfield Village, Ireland", Email = "test@test.dk", Name = "James On", Phone = "12345678"  });
            var response = new CustomerResponse() { Customers = customers.ToArray() };

            client.Setup(s => s.getCustomers(It.IsAny<CustomerRequest>())).Returns(response);

            var api = new CustomerSoapService(client.Object, config.Object);

            var result = api.GetCustomer();

            Assert.Equal("73", result.AccountNum);
            Assert.Equal("Bow St, Smithfield Village, Ireland", result.Address);
            Assert.Equal("test@test.dk", result.Email);
            Assert.Equal("James On", result.Name);
            Assert.Equal("12345678", result.Phone);
        }
    }
}
