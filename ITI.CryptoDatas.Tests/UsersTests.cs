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
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;

namespace ITI.CryptoDatas.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private HttpClient _client;

        public UsersTests()
        {
            _client = TestsHelper.InitializeServer();
        }

        [TestCase("toto", "tata", "tutu", "tototutu")]
        [TestCase("titi", null, "tutu", "tatatatata")]
        [TestCase("tete", "tonton", null, "tytytytytyt")]
        public async Task can_create_an_user_and_save_in_database(string username, string firstname, string lastname, string password)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user = new User()
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                Password = password
            };

            var response = await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);

            List<User> users = JsonHelper.GetFromDatabase<User>("users");
            User userGetted = users.Single(x => x.Username == user.Username);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(userGetted.Firstname, Is.EqualTo(user.Firstname));
            Assert.That(userGetted.Lastname, Is.EqualTo(user.Lastname));
            Assert.That(userGetted.Password, Is.Not.EqualTo(user.Password));
        }

        [TestCase("titi", "tatatatata", "titi", "tototototoo")]
        [TestCase("somename", "azerty", "somename", "123456")]
        public async Task return_unproccessable_entity_for_user_already_registered(string user1name, string user1pass, string user2name, string user2pass)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user1 = new User() { Username = user1name, Password = user1pass };
            User user2 = new User() { Username = user2name, Password = user2pass };

            await TestsHelper.SendRequest(_client, user1, "api/users/register", HttpMethod.Post);
            var response = await TestsHelper.SendRequest(_client, user2, "api/users/register", HttpMethod.Post);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [TestCase("popo", "popopupup")]
        [TestCase("pepe", "papappapapapa")]
        [TestCase("pupu", "spsppspspsps")]
        public async Task user_can_login(string username, string password)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user = new User() { Username = username, Password = password };

            await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);
            var response = await TestsHelper.SendRequest(_client, user, "api/users/login", HttpMethod.Post);
            User userResponse = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.That(userResponse.Username, Is.EqualTo(username));
        }

        [TestCase("toto", "tototutu")]
        [TestCase("titi", "tatatatata")]
        public async Task can_get_jwt_token_when_login_and_register(string username, string password)
        {
            TestsHelper.ClearDatabases();

            User user = new User() { Username = username, Password = password };

            var responseRegister = await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);
            var responseLogin = await TestsHelper.SendRequest(_client, user, "api/users/login", HttpMethod.Post);

            User resultRegister = JsonConvert.DeserializeObject<User>(await responseRegister.Content.ReadAsStringAsync());
            User resultLogin = JsonConvert.DeserializeObject<User>(await responseLogin.Content.ReadAsStringAsync());
            string claimsUsernameLogin = TestsHelper.GetUniqueNameToken(resultLogin.Token);
            string claimsUsernameRegister = TestsHelper.GetUniqueNameToken(resultRegister.Token);

            Assert.That(claimsUsernameLogin, Is.EqualTo(claimsUsernameRegister));
            Assert.That(resultRegister.Token, Is.EqualTo(resultLogin.Token));
        }

        [TestCase("username", "firstname", "lastname", "password")]
        [TestCase("username2", "firstname2", "lastname2", "password2")]
        [TestCase("username3", "firstname3", "lastname3", "password3")]
        [TestCase("username4", "firstname4", "lastname4", "password4")]
        public async Task can_edit_and_remove_an_user(string username, string firstname, string lastname, string password)
        {
            if(_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user = new User()
            {
                Username = username,
                Firstname = firstname,
                Lastname = lastname,
                Password = password
            };

            // REGISTER
            var response = await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);
            List<User> users = JsonHelper.GetFromDatabase<User>("users");
            User userGetted = users.Single(x => x.Username == user.Username);

            // EDIT
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + userGetted.Token);
            user.Lastname = "LastNameYolo";
            response = await TestsHelper.SendRequest(_client, user, "api/users/", HttpMethod.Put);

            users = JsonHelper.GetFromDatabase<User>("users");
            userGetted = users.Single(x => x.Username == user.Username);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(userGetted.Lastname, user.Lastname);

            // DELETE
            response = await TestsHelper.SendRequest(_client, user, "api/users/", HttpMethod.Delete);

            users = JsonHelper.GetFromDatabase<User>("users");
            Assert.AreEqual(users.Count, 0);
        }

        
    }
}
