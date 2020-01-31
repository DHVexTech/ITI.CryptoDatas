using ITI.CryptoDatas.Enums;
using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Managers
{
    public class WalletsManager
    {
        private readonly string _databaseName;

        public WalletsManager()
        {
            _databaseName = "wallets";
        }

        public List<int> Create()
        {

            throw new NotImplementedException();
        }

        public Wallet GetWallet(int walletId)
        {
            throw new NotImplementedException();
        }

        public bool Add (Wallet wallet) 
        {
            throw new NotImplementedException();
        }

        public Wallet ManageFund(Wallet refundProps, User currentUser, string @operator)
        {
            throw new NotImplementedException();
        }

        public List<Wallet> GetUserWallets(User user)
        {
            throw new NotImplementedException();
        }

        public Wallet GetSpecificWallet(User user, string crypto)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int walletId)
        {
            throw new NotImplementedException();

        }
        
    }
}
