using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using ITI.CryptoDatas;
using System.Net.Http;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;

namespace ITI.CryptoDatas.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private readonly HttpClient _client;

        public UsersTests()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            
            TestServer server = new TestServer(builder);
            _client = server.CreateClient();
        }

        [TestCase("toto", "tata", "tutu", "tototutu")]
        public async Task can_create_an_user(string username, string firstname, string lastname, string password)
        {
            User user = new User()
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                Password = password
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new ByteArrayContent(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(user)));
            var response = await _client.SendAsync(request);
            
            List<User> users = JsonHelper.GetFromDatabase<User>("users");
            User userGetted = users.Single(x => x.Username == user.Username);

            response.EnsureSuccessStatusCode();
            Assert.Equals(HttpStatusCode.OK, response.StatusCode);
            Assert.That(userGetted.Firstname, Is.EqualTo(user.Firstname));
            Assert.That(userGetted.Lastname, Is.EqualTo(user.Lastname));
            Assert.That(userGetted.Password, Is.Not.EqualTo(user.Password));
        }

        //[Test]
        //public void user_can_login()
        //{

        //}

        //[Test]
        //public void can_get_jwt_token_when_login_and_register()
        //{

        //}

        //[Test]
        //public void can_edit_and_remove_an_user()
        //{

        //}
        
    }
}
