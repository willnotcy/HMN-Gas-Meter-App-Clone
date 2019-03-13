using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using Xamarin.Forms;

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
            var date = DateTime.Now;

            var request = new InstallationRequest() { AccountNum = _config.CustomerId, Fom = date, UserContext = _config.Context, AttachmentNum = "", ContractNum = "", DeliveryCategory = "" };

            var response = _client.getInstallations(request);

            return (true, response.Installations[0]);
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
