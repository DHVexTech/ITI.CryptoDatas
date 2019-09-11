using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

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
    }
}
