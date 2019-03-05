using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using HMNGasApp.iOS.HMNGasnet;
using HMNGasApp.iOS.Services;
using HMNGasApp.Model;
using HMNGasApp.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CustomerSoapService))]
namespace HMNGasApp.iOS.Services
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
            var context = new HMNGasnet.UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = config.SecurityKey };

            var result = service.getCustomers(new CustomerRequest { AccountNum = config.CustomerId, UserContext = context, OrgNo = "" });

            return FromXellentCustomer(result.Customers[0]);
        }

        public Model.Customer FromXellentCustomer(HMNGasnet.Customer customer)
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