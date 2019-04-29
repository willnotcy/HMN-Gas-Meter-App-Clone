using System.Collections.Generic;
using HMNGasApp.WebServices;

namespace HMNGasApp.Model
{
    public class Config : IConfig
    {
        public string ApiKey { get; set; }

        public string CustomerId { get; set; }

        public UserContext Context { get; set; }

        public List<MeterReading> MeterReadings { get; set; }
    }
}
