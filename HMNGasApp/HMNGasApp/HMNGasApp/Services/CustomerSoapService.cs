using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HMNGasApp.Services
{
    /// <summary>
    /// Class for handling request regarding retrieval and update of customer information
    /// </summary>
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
        public async Task<(bool, WebServices.Customer)> GetCustomerAsync()
        {
            return await Task.Run(() =>
            {
                var context = _config.Context;

                var result = _client.getCustomers(new CustomerRequest { AccountNum = _config.CustomerId, UserContext = context, OrgNo = "" });

                if (result != null && result.Customers.Length == 1)
                {
                    return (true, result.Customers[0]);
                }
                else
                {
                    return (false, null);
                }
            }
            );
        }

        /// <summary>
        /// Updates information regarding input customer
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        public async Task<bool> EditCustomerAsync(WebServices.Customer Customer)
        {
            return await Task.Run(() =>
            {
                var context = _config.Context;

                var result = _client.newCustContactInfo(new NewCustContactInfoRequest
                {
                    AccountNum = Customer.AccountNum,
                    AlternativeCustomerCode = Customer.AlternativeCustomerCode,
                    AlternativeCustomerNumber = Customer.AlternativeCustomerNumber,
                    BirthDate = Customer.BirthDate,
                    CellPhone = Customer.CellularPhone,
                    City = Customer.City,
                    CustBankAcc = Customer.CustBankAcc,
                    Email = Customer.Email,
                    LanguageId = Customer.LanguageId,
                    Name = Customer.Name,
                    NetsShareToEbox = Customer.NetsShareToEbox,
                    Phone = Customer.Phone,
                    PostOfficeBox = Customer.PostOfficeBox,
                    Residence = Customer.Residence,
                    SecondaryCellPhone = Customer.SecondaryCellularPhone,
                    SecondaryEmail = Customer.SecondaryEmail,
                    Street = Customer.Street,
                    TeleFax = Customer.TeleFax,
                    ZipCode = Customer.ZipCode,
                    UserContext = context
                });
                if (result != null && result.ResponseCode.Equals("Ok"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
    }
}
