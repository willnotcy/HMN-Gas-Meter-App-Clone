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
        private static readonly Uri _backendUrl = new Uri("https://localhost:44336/");

        public JSONRepository()
        {
            var handler = new HttpClientHandler();

            //try
            //{
            //    handler.ServerCertificateCustomValidationCallback += (message, cert, chain, errors) => true;
            //} catch (Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine(e.StackTrace);
            //}
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _client = new HttpClient(handler) { BaseAddress = _backendUrl };
            
        }

        public async Task<string> Read()
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.GetAsync($"api/file");
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

            return await response.Content.ReadAsStringAsync();
        } 
    }
}
