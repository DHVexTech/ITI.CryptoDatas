﻿using ITI.CryptoDatas.Helpers;
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
using System.Net.Http.Headers;

namespace ITI.CryptoDatas.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private HttpClient _client;

        public UsersTests()
        {
            var builder = new WebHostBuilder()
                          .UseEnvironment("Development")
                          .UseStartup<Startup>();
            
            TestServer server = new TestServer(builder);
            _client = server.CreateClient();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TestCase("toto", "tata", "tutu", "tototutu")]
        [TestCase("titi", null, "tutu", "tatatatata")]
        [TestCase("tete", "tonton", null, "tytytytytyt")]
        public async Task can_create_an_user_and_save_in_database(string username, string firstname, string lastname, string password)
        {
            User user = new User()
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                Password = password
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            
            List<User> users = JsonHelper.GetFromDatabase<User>("users");
            User userGetted = users.Single(x => x.Username == user.Username);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(userGetted.Firstname, Is.EqualTo(user.Firstname));
            Assert.That(userGetted.Lastname, Is.EqualTo(user.Lastname));
            Assert.That(userGetted.Password, Is.Not.EqualTo(user.Password));
            JsonHelper.WriteInDatabase<User>(new List<User>(), "users");
        }

        [TestCase("titi", "tatatatata", "titi", "tototototoo")]
        [TestCase("somename", "azerty", "somename", "123456")]
        public async Task return_unproccessable_entity_for_user_already_registered(string user1name, string user1pass, string user2name, string user2pass)
        {
            User user1 = new User() { Username = user1name, Password = user1pass };
            User user2 = new User() { Username = user2name, Password = user2pass };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user1), Encoding.UTF8, "application/json");
            await _client.SendAsync(request);

            request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.SendAsync(request);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            JsonHelper.WriteInDatabase<User>(new List<User>(), "users");
        }

        [TestCase("popo", "popopupup")]
        [TestCase("pepe", "papappapapapa")]
        [TestCase("pupu", "spsppspspsps")]
        public async Task user_can_login(string username, string password)
        {
            User user = new User(){Username = username,Password = password};

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await _client.SendAsync(request);
            request = new HttpRequestMessage(HttpMethod.Post, "api/users/login");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.SendAsync(request);
            string result = response.Content.ReadAsStringAsync().Result;

            User userResponse = JsonConvert.DeserializeObject<User>(result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(userResponse.Username, Is.EqualTo(username));
        }

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
