using System;
using System.Collections.Generic;
using System.Text;

namespace HMNGasApp.Model
{
    public class Config : IConfig
    {
        public string ApiKey { get; set; }

        public string SecurityKey { get; set; }

        public string CustomerId { get; set; }
    }
}
