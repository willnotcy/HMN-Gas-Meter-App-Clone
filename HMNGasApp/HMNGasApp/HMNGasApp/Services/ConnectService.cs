using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    /// <summary>
    /// This class has been added in order to check if the API is ready to receive requests. 
    /// By calling canConnect, it returns true or false if the the api is ready or not
    /// </summary>
    public class ConnectService : IConnectService
    {
        private readonly string Firm = "HNG";
        private IXellentAPI _client;

        public ConnectService(IXellentAPI Client)
        {
            _client = Client;
        }

        public async Task<bool> CanConnect()
        {
            return await Task.Run(() =>
            {
                var canConnect = false;
                //HACK: Should maybe be changed or at least reasoned why 3 tries is great?
                for (int i = 0; i < 3; i++)
                {
                    var result = _client.canConnect(Firm);
                    if (result)
                    {
                        canConnect = true;
                        break;
                    }
                }

                return canConnect;
            });
        }
    }
}
