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
            throw new NotImplementedException();
        }
    }
}
