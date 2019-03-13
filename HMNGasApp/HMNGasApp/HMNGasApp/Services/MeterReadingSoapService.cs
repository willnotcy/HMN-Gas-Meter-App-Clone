using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public class MeterReadingSoapService : IMeterReadingSoapService
    {
        private readonly IXellentAPI _client;
        private readonly IConfig _config;

        public MeterReadingSoapService(IXellentAPI client, IConfig config)
        {
            _client = client;
            _config = config;
        }

        public (bool, Installation) GetInstallations()
        {
            throw new NotImplementedException();
        }

        public (bool, MeterReadingOrderResponse) GetMeterReadingOrder(Installation installation)
        {
            throw new NotImplementedException();
        }

        public (bool, string) NewMeterReading()
        {
            throw new NotImplementedException();
        }
    }
}
