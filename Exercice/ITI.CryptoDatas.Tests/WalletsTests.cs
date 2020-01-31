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
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();

            User user = new User() { Username = username, Password = password };

            var response = await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);

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

            var response = await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);
            user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user.Token);
            response = await TestsHelper.SendRequest(_client, wallet, "api/wallets/refund", HttpMethod.Put);

            response = await TestsHelper.SendRequest(_client, user, "api/wallets/" + crypto, HttpMethod.Get);
            Wallet userWallet = JsonConvert.DeserializeObject<Wallet>(response.Content.ReadAsStringAsync().Result);

            Assert.That(userWallet.Fund, Is.EqualTo(amount));

            response = await TestsHelper.SendRequest(_client, wallet, "api/wallets/removeFund", HttpMethod.Put);

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

            var response = await TestsHelper.SendRequest(_client, user, "api/users/register", HttpMethod.Post);
            user = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");

            Assert.That(wallets.Count, Is.Not.EqualTo(0));

            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user.Token);
            response = await TestsHelper.SendRequest(_client, user, "api/users/", HttpMethod.Delete);

            wallets = JsonHelper.GetFromDatabase<Wallet>("wallets");

            Assert.That(wallets.Count, Is.EqualTo(0));
        }
    }
}
