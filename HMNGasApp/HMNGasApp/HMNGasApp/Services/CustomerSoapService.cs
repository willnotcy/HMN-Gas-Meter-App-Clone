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

        /// <summary>
        /// Obtains customer information for the current customer
        /// </summary>
        /// <returns>Customer object</returns>
        public async Task<WebServices.Customer> GetCustomerAsync()
        {
            var context = new WebServices.UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = _config.SecurityKey };

            var result = _client.getCustomers(new CustomerRequest { AccountNum = _config.CustomerId, UserContext = context, OrgNo = "" });

            return result.Customers[0];
        }

        public async Task<bool> EditCustomerAsync(WebServices.Customer customer)
        {
            var context = new WebServices.UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = _config.SecurityKey };

            var result = _client.newCustContactInfo(new NewCustContactInfoRequest { AccountNum = customer.AccountNum, AlternativeCustomerCode = customer.AlternativeCustomerCode,
                                                                                    AlternativeCustomerNumber = customer.AlternativeCustomerNumber, BirthDate = customer.BirthDate,
                                                                                    CellPhone = customer.CellularPhone, City = customer.City, CustBankAcc = customer.CustBankAcc,
                                                                                    Email = customer.Email, LanguageId = customer.LanguageId, Name = customer.Name,
                                                                                    NetsShareToEbox = customer.NetsShareToEbox, Phone = customer.Phone, PostOfficeBox = customer.PostOfficeBox,
                                                                                    Residence = customer.Residence, SecondaryCellPhone = customer.SecondaryCellularPhone,
                                                                                    SecondaryEmail = customer.SecondaryEmail, Street = customer.Street, TeleFax = customer.TeleFax,
                                                                                    ZipCode = customer.ZipCode, UserContext = context });
            if (result.ResponseCode.Equals("Ok"))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
