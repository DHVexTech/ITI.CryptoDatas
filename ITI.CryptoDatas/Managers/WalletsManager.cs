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
        public UsersManager _userManager;
        private readonly string _databaseName;

        public WalletsManager(UsersManager userManager)
        {
            _userManager = userManager;
            _databaseName = "wallets";
        }

        public Wallet GetWallet(int walletId)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            if (wallets.Count > 0)
            {
                Wallet wallet = wallets.First(x => x.Id == walletId);
                if (wallet != null)
                    return wallet;
            }
            return null;
        }

        public bool Add (Wallet wallet) 
        {
            List<Wallet> list = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            list.Add(wallet);
            JsonHelper.WriteInDatabase<Wallet>(list, _databaseName);
            return true;
        }

        public Wallet Refund(Wallet wallet, string username)
        {
            double currentFund = 0;
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);


            return wallet;
        }

        public bool Delete(int walletId)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            Wallet wallet = wallets.First(x => x.Id == walletId);
            if (wallets.Remove(wallet))
            {
                JsonHelper.WriteInDatabase<Wallet>(wallets, _databaseName);
                return true;
            }
            return false;
        }
    }
}
