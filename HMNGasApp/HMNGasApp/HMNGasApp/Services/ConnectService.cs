using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMNGasApp.Services
{
    public class ConnectService : IConnectService
    {
        private readonly string Firm = "HNG";
        private IXellentAPI _client;
        private IConfig _config;

        public ConnectService(IXellentAPI Client)
        {
            _client = Client;
        }

        public bool canConnect()
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
