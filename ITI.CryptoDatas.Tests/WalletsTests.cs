using ITI.CryptoDatas.Enums;
using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Tests
{
    [TestFixture]
    public class WalletsTests
    {
        private HttpClient _client;

        public WalletsTests()
        {
            _client = TestsHelper.InitializeServer();
        }

        [TestCase("toto", "tototutu")]
        [TestCase("titi", "tatatatata")]
        public async Task initialize_wallet_when_a_user_is_created(string username, string password)
        {
            TestsHelper.ClearDatabases();

            User user = new User() { Username = username, Password = password };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");
            List<User> users = JsonHelper.GetFromDatabase<User>("users");
            User userGetted = users.First(x => x.Username == username);

            Assert.That(userGetted.Wallets, Is.Not.Null);
            Assert.That(wallets.Count, Is.EqualTo(Enum.GetNames(typeof(CryptoEnum)).Length-1));
        }

        [TestCase("popol", "diu5Gsn", "BTC", 0.55)]
        [TestCase("tutu", "UdnE5", "ETH", 0.04)]
        [TestCase("titititi", "tytytutu", "BCH", 0.0001)]
        [TestCase("opu", "cxcc", "DOGE", 0.05)]
        [TestCase("thuthu", "polde", "XRP", 1)]
        [TestCase("popolo", "tersyUj", "LTC", 152)]
        public async Task add_and_remove_fund(string username, string password, string crypto, double amount)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user = new User() { Username = username, Password = password };
            Wallet wallet = new Wallet() { CryptoName = crypto, Fund = amount };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var responseUser = await _client.SendAsync(request);
            user = JsonConvert.DeserializeObject<User>(responseUser.Content.ReadAsStringAsync().Result);

            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");
            request = new HttpRequestMessage(HttpMethod.Put, "api/wallets/refund");
            request.Content = new StringContent(JsonConvert.SerializeObject(wallet), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user.Token);
            var response = await _client.SendAsync(request);

            request = new HttpRequestMessage(HttpMethod.Get, "api/wallets/" + crypto);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            Wallet userWallet = JsonConvert.DeserializeObject<Wallet>(response.Content.ReadAsStringAsync().Result);

            Assert.That(userWallet.Fund, Is.EqualTo(amount));

            request = new HttpRequestMessage(HttpMethod.Put, "api/wallets/removeFund");
            request.Content = new StringContent(JsonConvert.SerializeObject(wallet), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            userWallet = JsonConvert.DeserializeObject<Wallet>(response.Content.ReadAsStringAsync().Result);

            Assert.That(userWallet.Fund, Is.EqualTo(0));
        }

        [TestCase("toto", "tototutu")]
        [TestCase("titi", "tatatatata")]
        public async Task remove_all_wallets_when_an_user_is_deleted(string username, string password)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user = new User() { Username = username, Password = password };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/users/register");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            user = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");

            Assert.That(wallets.Count, Is.Not.EqualTo(0));

            request = new HttpRequestMessage(HttpMethod.Delete, "api/users?username=" + user.Username);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user.Token);
            response = await _client.SendAsync(request);

            wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");

            Assert.That(wallets.Count, Is.EqualTo(0));
        }
    }
}
