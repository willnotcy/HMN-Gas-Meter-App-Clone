using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HMNGasApp.Services
{
    public class CustomerSoapService : ICustomerSoapService
    {
        private readonly IXellentAPI _client;
        private readonly IConfig _config;

        public CustomerSoapService(IXellentAPI client, IConfig config)
        {
            _client = client;
            _config = config;
        }

        public Model.Customer GetCustomer()
        {
            var context = new WebServices.UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = _config.SecurityKey };

            var result = _client.getCustomers(new CustomerRequest { AccountNum = _config.CustomerId, UserContext = context, OrgNo = "" });

            return FromXellentCustomer(result.Customers[0]);
        }

        public Model.Customer FromXellentCustomer(WebServices.Customer customer)
        {
            return new Model.Customer()
            {
                AccountNum = customer.AccountNum,
                Address = customer.Address,
                Email = customer.Email,
                Name = customer.Name,
                Phone = customer.Phone
            };
        }
    }
}
