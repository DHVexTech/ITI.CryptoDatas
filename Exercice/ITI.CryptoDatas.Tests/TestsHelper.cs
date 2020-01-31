using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Tests
{
    public static class TestsHelper
    {
        public static HttpClient InitializeServer()
        {
            HttpClient client;
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();

            TestServer server = new TestServer(builder);
            client = server.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static string GetUniqueNameToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            return token.Claims.First(x => x.Type == "unique_name").Value;
        }

        public static void ClearDatabases()
        {
            JsonHelper.WriteInDatabase<Wallet>(new List<Wallet>(), "wallets");
            JsonHelper.WriteInDatabase<User>(new List<User>(), "users");
            JsonHelper.WriteInDatabase<Wallet>(new List<Wallet>(), "wallets");
            JsonHelper.WriteInDatabase<Transaction>(new List<Transaction>(), "transactions");
        }

        public static async Task<HttpResponseMessage> SendRequest(HttpClient client, object body, string query, HttpMethod method = null)
        {
            if (method == null)
                method = HttpMethod.Post;
            HttpRequestMessage request = new HttpRequestMessage(method, query);
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            return await client.SendAsync(request);
        }
    }
}
