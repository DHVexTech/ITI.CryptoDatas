using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Helpers
{
    public static class HttpHelper
    {
        public static async Task<string> SendRequest(string baseUri, string query, string apikey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apikey);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, query);
                HttpResponseMessage response = await client.SendAsync(request);
                Stream receiveStream = await response.Content.ReadAsStreamAsync();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                return readStream.ReadToEnd();
            }
        }

        public static async Task<string> SendRequest(string baseUri, string query, string body, HttpMethod httpMethod, string token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("accept", "bearer " + token);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, query);
                request.Content = new ByteArrayContent(Encoding.ASCII.GetBytes(body));
                HttpResponseMessage response = await client.SendAsync(request);
                Stream receiveStream = await response.Content.ReadAsStreamAsync();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                return readStream.ReadToEnd();
            }
        }
    }
}
