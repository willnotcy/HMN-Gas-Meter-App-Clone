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
            try
            {
                var response = await _client.GetAsync("text.json");
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
                throw e;
            }
        } 
    }
}
