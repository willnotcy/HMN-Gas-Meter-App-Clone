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
        private readonly IConnectService _connectService;

        public LoginSoapService(IXellentAPI client, IConnectService connectService, IConfig config)
        {
            _client = client;
            _config = config;
            _connectService = connectService;
        }

        /// <summary>
        /// Attempts to obtain a new login from the given parameters.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="password"></param>
        /// <returns>Tuple of success and security key</returns>
        public async Task<(bool, string)> NewLogin(string customerId, string password)
        {
            var key = SHA.SHA1Encrypt(string.Format("{0}{1}", customerId, _config.ApiKey));

            var canConnect = _connectService.canConnect();

            if (canConnect)
            {
                (bool, string) result = (false, "Not ok");

                for(int i = 0; i < 3; i++)
                {
                    var response = _client.newLogin(new NewLoginRequest() { NewLogin = new NewLogin { WebLogin = customerId, PassWord = password, EncryptedKey = key } });

                    result = response.ErrorCode.Equals("") ? (true, response.ResponseMessage) : (false, response.ResponseCode);

                    if (result.Item1)
                    {
                        _config.SecurityKey = result.Item2;
                        _config.CustomerId = customerId;

                        return result;
                    }
                }

                return result;
            }

            return (false, "Kunne ikke få forbindelse");
        }

        public async Task<bool> Logout()
        {
            var result = _client.logout(new LogoutRequest { WebLogin = _config.CustomerId, UserContext = new WebServices.UserContext { Caller = "", Company = "", functionName = "", Logg = 0, MaxRows = 1, StartRow = 0, securityKey = _config.SecurityKey } });

            _config.SecurityKey = "";

            return result.ErrorCode.Equals("");
        }
    }
}
