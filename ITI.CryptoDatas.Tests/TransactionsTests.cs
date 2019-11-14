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

namespace ITI.CryptoData.Tests
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

            var response = await TestsHelper.SendRequest(_client, user1, "api/users/register");
            // get user
           response = await TestsHelper.SendRequest(_client, user2, "api/users/register");
            // get user 2
            _client.DefaultRequestHeaders.Add("Authorization", "bearer " + user1.Token);
            await TestsHelper.SendRequest(_client, fundUser1, "api/wallets/refund");
            await TestsHelper.SendRequest(_client, transaction, "api/transactions/give");
            response = await TestsHelper.SendRequest(_client, transaction, "api/wallets/" + crypto);


            
        }

        [Test]
        public void save_transaction_in_database()
        {

        }
    }
}
