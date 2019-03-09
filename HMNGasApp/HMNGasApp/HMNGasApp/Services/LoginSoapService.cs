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
        private readonly IXellentAPI _client;
        private readonly IConfig _config;

        public LoginSoapService(IXellentAPI client, IConfig config)
        {
            _client = client;
            _config = config;
        }

        public async Task<(bool, string)> NewLogin(string customerId, string password)
        {
            var key = SHA.SHA1Encrypt(string.Format("{0}{1}", customerId, _config.ApiKey));

            var response = _client.newLogin(new NewLoginRequest() { NewLogin = new NewLogin { WebLogin = customerId, PassWord = password, EncryptedKey = key } });

            var result = response.ErrorCode.Equals("") ? (true, response.ResponseMessage) : (false, response.ResponseCode);

            if (result.Item1)
            {
                _config.SecurityKey = result.Item2;
                _config.CustomerId = customerId;
            }

            return result;
        }
    }
}
