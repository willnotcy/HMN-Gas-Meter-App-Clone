using System.Threading.Tasks;
using HMNGasApp.Helpers;
using HMNGasApp.iOS;
using HMNGasApp.iOS.HMNGasnet;
using HMNGasApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginSoapService))]
namespace HMNGasApp.iOS
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
            var key = SHA.SHA1Encrypt(string.Format("{0}{1}", customerId, ""));

            var response = service.newLogin(new NewLoginRequest() { NewLogin = new NewLogin { WebLogin = customerId, PassWord = password, EncryptedKey = key } });

            var result = response.ErrorCode.Equals("") ? (true, response.ResponseMessage) : (false, response.ResponseCode);

            return result;
        }
    }
}