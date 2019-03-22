using HMNGasApp.WebServices;
using System.Collections.Generic;

namespace HMNGasApp.Model
{
    public interface IConfig
    {
        string ApiKey { get; set; }

        string SecurityKey { get; set; }

        string CustomerId { get; set; }

        string Name { get; set; }

        UserContext Context { get; set; }

        List<MeterReading> MeterReadings { get; set; } 
    }
}