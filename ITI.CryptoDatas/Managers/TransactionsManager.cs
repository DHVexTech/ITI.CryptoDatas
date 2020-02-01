using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ITI.CryptoDatas.Managers
{
    public class TransactionsManager
    {
        private readonly UsersManager _usersManager;
        private readonly WalletsManager _walletsManager;

        public TransactionsManager(UsersManager usersManager, WalletsManager walletsManager)
        {
        }

        public Transaction Give(string usernameGiver, Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
