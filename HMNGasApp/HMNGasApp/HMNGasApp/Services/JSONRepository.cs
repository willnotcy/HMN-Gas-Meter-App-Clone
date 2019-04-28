using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        public async Task<string> Read()
        {
            var response = await _client.GetAsync($"api/file");

            return await response.Content.ReadAsStringAsync();
        } 
    }
}
