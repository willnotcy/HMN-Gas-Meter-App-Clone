using HMNGasApp.Model;
using HMNGasApp.WebServices;
using System;
using System.Linq;
using System.Collections.Generic;
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

        /// <summary>
        /// Sends a new meter reading to the xellent api.
        /// </summary>
        /// <param name="reading"></param>
        /// <returns>status and responsecode</returns>
        public async Task<(bool, string)> NewMeterReadingAsync(string reading)
        {
            return await Task.Run(async () =>
            {
                var active = await GetActiveMeterReadingsAsync();

                if (!active.Item1)
                    return (false, "Du har ingen gyldige aflæsningskort.");

                var activeValues = active.Item2;

                if (float.Parse(reading) < float.Parse(activeValues.PrevReading.Replace(",", ".")))
                    return (false, "Din måling kan ikke være lavere end sidste års måling.");

                var readings = new List<NewMeterReading>()
                {
                    new NewMeterReading
                    {
                        AccountNum = _config.CustomerId,
                        InstNum = activeValues.InstNum,
                        DeliveryCategory = activeValues.DeliveryCategory,
                        CompanyId = activeValues.CompanyId,
                        CounterNum = activeValues.CounterNum,
                        DelPointNum = activeValues.DelPointNum,
                        MeterJournalId = activeValues.MeterJournalId,
                        MeterNum = activeValues.MeterNum,
                        QuantityCodeValue = activeValues.QuantityCodeValue,
                        Reading = reading,
                        ReadingCardLine = activeValues.ReadingCardLine,
                        ReadingCardNum = activeValues.ReadingCardNum,
                        ReadingDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        ReadingMethodNum = activeValues.ReadingMethodNum,
                        ReadingStatus = activeValues.ReadingStatus,
                        ReasonToReading = activeValues.ReasonToReading
                    }
                };

                var request = new NewMeterReadingRequest()
                {
                    UserContext = _config.Context,
                    NewMeterReadings = readings.ToArray()
                };

                var response = _client.newMeterReading(request);

                var result = response.ErrorCode.Equals("") ? (true, response.ResponseMessage) : (false, response.ResponseCode);

                return result;
            });
        }

        /// <summary>
        /// Returns any active meter reading orders.
        /// </summary>
        /// <returns>status and MeterReadingOrder object</returns>
        public async Task<(bool, MeterReadingOrder)> GetActiveMeterReadingsAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var request = new ActiveMeterReadingRequest()
                    {
                        AccountNum = _config.CustomerId,
                        UserContext = _config.Context
                    };

                    var response = _client.getactiveMeterReadings(request);

                    return (true, response.MeterReadingOrders[0]);
                }
                //TODO: Hmmmmm...?
                catch (Exception)
                {
                    return (false, null);
                }
            });
        }

        /// <summary>
        /// Returns previous meter readings
        /// </summary>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <returns>List of meter readings</returns>
        public async Task<(bool, List<MeterReading>)> GetMeterReadings(DateTime from, DateTime to)
        {
            if((to - from).TotalMilliseconds > 0)
            {
                return await Task.Run(() =>
                {
                    var request = new MeterReadingsRequest
                    {
                        AccountNum = _config.CustomerId,
                        Fom = from,
                        ToDate = to,
                        UserContext = _config.Context,
                        AttachmentNum = "",
                        DeliveryCategory = ""
                    };

                    var response = _client.getMeterReadings(request);

                    return (true, response.MeterReadings.ToList());
                });
            }
            else
            {
                //TODO: What happens here?
                return (false, default(List<MeterReading>));
            }
        }
    }
}
