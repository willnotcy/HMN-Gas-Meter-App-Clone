using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HMNGasApp.Services
{
    public class JSONRepository : IJSONRepository
    {
        private readonly HttpClient _client;

        public JSONRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<Dictionary<string, string>> Read()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.hmn.json");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                Dictionary<string, string> ValueList = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                return ValueList;
            }
        } 
    }
}
