using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using ITI.CryptoDatas.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Tests
{
    [TestFixture]
    public class TransactionsTests
    {
        private HttpClient _client;

        public TransactionsTests()
        {
            _client = TestsHelper.InitializeServer();
        }

        [TestCase(0.5, "BTC")]
        [TestCase(5.1, "LTC")]
        [TestCase(15.2, "ETH")]
        public async Task can_do_transaction_to_another_user(double amount, string crypto)
        {
            if (_client.DefaultRequestHeaders.Contains("Authorization")) _client.DefaultRequestHeaders.Remove("Authorization");
            TestsHelper.ClearDatabases();
            User user1 = new User() { Username = "Toto", Password = "totoestfort" };
            User user2 = new User() { Username = "Titi", Password = "titiestforte" };
            Wallet fundUser1 = new Wallet() { CryptoName = crypto, Fund = 100 };
            Transaction transaction = new Transaction() { Crypto = crypto, Fund = amount, UsernameGiver = user1.Username, UsernameReceiver = user2.Username };

            var responseUser1 = await TestsHelper.SendRequest(_client, user1, "api/users/register");
            user1 = JsonConvert.DeserializeObject<User>(await responseUser1.Content.ReadAsStringAsync());
            // get user
            var responseUser2 = await TestsHelper.SendRequest(_client, user2, "api/users/register");
            user2 = JsonConvert.DeserializeObject<User>(await responseUser2.Content.ReadAsStringAsync());
            // get user 2
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user1.Token);
            await TestsHelper.SendRequest(_client, fundUser1, "api/wallets/refund", HttpMethod.Put);
            await TestsHelper.SendRequest(_client, transaction, "api/transactions/give");
            var responseWallet = await TestsHelper.SendRequest(_client, null, "api/wallets/" + crypto, HttpMethod.Get);
            fundUser1 = JsonConvert.DeserializeObject<Wallet>(await responseWallet.Content.ReadAsStringAsync());

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user2.Token);
            var responseWallet2 = await TestsHelper.SendRequest(_client, null, "api/wallets/" + crypto, HttpMethod.Get);
            Wallet fundUser2 = JsonConvert.DeserializeObject<Wallet>(await responseWallet2.Content.ReadAsStringAsync());

            Assert.That(fundUser1.Fund == 100 - amount);
            Assert.That(fundUser2.Fund == amount);
        }
    }
}
