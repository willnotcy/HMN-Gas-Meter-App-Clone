using HMNGasApp.WebServices;

namespace HMNGasApp.Model
{
    public class Config : IConfig
    {
        public string ApiKey { get; set; }

        public string SecurityKey { get; set; }

        public string CustomerId { get; set; }

        public string Name { get; set; }

        public UserContext Context { get; set; }
    }
}
