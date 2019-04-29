using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Text;

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

        public bool CanConnect()
        {
            var canConnect = false;

            for(int i = 0; i < 3; i++)
            {
                var result = _client.canConnect(Firm);
                if(result)
                {
                    canConnect = true;
                    break;
                }
            }

            return canConnect;
        }
    }
}
