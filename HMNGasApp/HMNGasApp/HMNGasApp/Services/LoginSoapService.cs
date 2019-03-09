using HMNGasApp.Helpers;
using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HMNGasApp.Services
{
    public class LoginSoapService : ILoginSoapService
    {
        XellentAPI service;

        public LoginSoapService()
        {
            service = new XellentAPI()
            {
                Url = "http://xel-webfront-test.gasnet.dk:81/TEST/XellentAPI.asmx"
            };
        }

        public async Task<(bool, string)> NewLogin(string customerId, string password)
        {
            var config = DependencyService.Resolve<IConfig>();
            var key = SHA.SHA1Encrypt(string.Format("{0}{1}", customerId, config.ApiKey));

            var response = service.newLogin(new NewLoginRequest() { NewLogin = new NewLogin { WebLogin = customerId, PassWord = password, EncryptedKey = key } });

            var result = response.ErrorCode.Equals("") ? (true, response.ResponseMessage) : (false, response.ResponseCode);

            if (result.Item1)
            {
                config.SecurityKey = result.Item2;
                config.CustomerId = customerId;
            }

            return result;
        }
    }
}
