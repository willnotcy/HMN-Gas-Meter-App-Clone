using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMNGasApp.Droid.HMNGasnet;
using HMNGasApp.Droid.Services;
using HMNGasApp.Model;
using HMNGasApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(CustomerSoapService))]
namespace HMNGasApp.Droid.Services
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

        public Task<List<Model.Customer>> GetCustomersAsync()
        {
            throw new NotImplementedException();
        }

        //public async Task<List<Model.Customer>> GetCustomersAsync()
        //{
        //    return await Task.Run(() =>
        //    {
        //        var context = new UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = "" };
        //        //var result = await service.getCustomersAsync(new CustomerRequest {AccountNum = });

        //        //return new List<Model.Customer>(result);

        //    });
        //}
    }
}