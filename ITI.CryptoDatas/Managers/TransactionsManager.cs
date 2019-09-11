using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using System.Collections.Generic;
using System.Linq;

namespace ITI.CryptoDatas.Managers
{
    public class TransactionsManager
    {
        private readonly UsersManager _usersManager;
        private readonly WalletsManager _walletsManager;
        private readonly string _databaseName;

        public TransactionsManager(UsersManager usersManager, WalletsManager walletsManager)
        {
            _databaseName = "transactions";
            _usersManager = usersManager;
            _walletsManager = walletsManager;
        }

        public Transaction Give(string usernameGiver, Transaction transaction)
        {
            transaction.UsernameGiver = usernameGiver;
            User receiver = _usersManager.GetUser(transaction.UsernameReceiver);
            User giver = _usersManager.GetUser(usernameGiver);
            Wallet walletTransaction = new Wallet()
            {
                CryptoName = transaction.Crypto,
                Fund = transaction.Fund
            };
            _walletsManager.ManageFund(walletTransaction, giver, "-");
            _walletsManager.ManageFund(walletTransaction, receiver, "+");

            List<Transaction> transactions = JsonHelper.GetFromDatabase<Transaction>(_databaseName);
            transaction.Id = transactions.Last().Id + 1;
            transactions.Add(transaction);
            JsonHelper.WriteInDatabase<Transaction>(transactions, _databaseName);
            return null;
        }
    }
}
