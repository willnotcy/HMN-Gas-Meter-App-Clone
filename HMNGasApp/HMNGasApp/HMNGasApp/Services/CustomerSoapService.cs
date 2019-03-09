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
        XellentAPI service;

        public CustomerSoapService()
        {
            service = new XellentAPI()
            {
                Url = "http://xel-webfront-test.gasnet.dk:81/TEST/XellentAPI.asmx"
            };
        }

        public async Task<Model.Customer> GetCustomerAsync()
        {
            var config = DependencyService.Resolve<IConfig>();
            var context = new WebServices.UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = config.SecurityKey };

            var result = service.getCustomers(new CustomerRequest { AccountNum = config.CustomerId, UserContext = context, OrgNo = "" });

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
