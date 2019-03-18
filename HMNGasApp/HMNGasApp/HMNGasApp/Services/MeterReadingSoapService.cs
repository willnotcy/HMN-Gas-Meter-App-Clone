using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Collections.Generic;
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

            if (response == null)
                return (false, null);

            return (true, response.Installations[0]);
        }

        public (bool, MeterReadingOrder) GetMeterReadingOrder(Installation installation, MeterReadingOrder active)
        {
            try
            {
                var date = DateTime.Now;

                var request = new MeterReadingOrderRequest() { AccountNum = _config.CustomerId, AttachmentNum = installation.AttachmentNum, Fom = date, DeliveryCategory = "", ReadingCardNum = active.ReadingCardNum, ToDate = date, UserContext = _config.Context };

                var response = _client.getMeterReadingOrder(request);

                if (response == null)
                    return (false, null);

                return (true, response.MeterReadingOrders[0]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public (bool, string) NewMeterReading(string reading)
        {
            var active = GetActiveMeterReadings();

            if (!active.Item1)
                return (false, "Du har ingen gyldige aflæsningskort.");

            var activeValues = active.Item2;

            if(float.Parse(reading) < float.Parse(activeValues.PrevReading.Replace(",", ".")))
                return (false, "Din måling kan ikke være lavere end sidste års måling.");
                
            var readings = new List<NewMeterReading>()
            {
                new NewMeterReading { AccountNum = _config.CustomerId, InstNum = activeValues.InstNum, DeliveryCategory = activeValues.DeliveryCategory, CompanyId = activeValues.CompanyId, CounterNum = activeValues.CounterNum, DelPointNum = activeValues.DelPointNum, MeterJournalId = activeValues.MeterJournalId, MeterNum = activeValues.MeterNum, QuantityCodeValue = activeValues.QuantityCodeValue, Reading = reading, ReadingCardLine = activeValues.ReadingCardLine, ReadingCardNum = activeValues.ReadingCardNum, ReadingDate = DateTime.Now.ToString("yyyy-MM-dd"), ReadingMethodNum = activeValues.ReadingMethodNum, ReadingStatus = activeValues.ReadingStatus, ReasonToReading = activeValues.ReasonToReading }
            };

            var request = new NewMeterReadingRequest() { UserContext = _config.Context, NewMeterReadings = readings.ToArray() };

            var response = _client.newMeterReading(request);

            var result = response.ErrorCode.Equals("") ? (true, response.ResponseMessage) : (false, response.ResponseCode);

            return result;
        }

        public (bool, MeterReadingOrder) GetActiveMeterReadings()
        {
            try
            {
                var request = new ActiveMeterReadingRequest() { AccountNum = _config.CustomerId, UserContext = _config.Context };

                var response = _client.getactiveMeterReadings(request);
                
                return (true, response.MeterReadingOrders[0]);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }
    }
}
