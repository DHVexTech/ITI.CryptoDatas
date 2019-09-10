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

        public Wallet ManageFund(Wallet refundProps, string username, string @operator)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            User currentUser = _userManager.GetUser(username);
            Wallet walletSelected = wallets.Single(x => x.CryptoName == refundProps.CryptoName && currentUser.Wallets.Contains(x.Id));
            wallets.Remove(walletSelected);
            if (@operator == "+")
                walletSelected.Fund -= refundProps.Fund;
            else
                walletSelected.Fund += refundProps.Fund;
            wallets.Add(walletSelected);
            JsonHelper.WriteInDatabase<Wallet>(wallets, _databaseName);
            walletSelected.Id = 0;
            return walletSelected;
        }

        public List<Wallet> GetUserWallets(User user)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            return wallets.Where(x => user.Wallets.Contains(x.Id)).ToList();
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

        private Wallet GetUserWallet(string username, string crypto)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            User currentUser = _userManager.GetUser(username);
            return wallets.Single(x => x.CryptoName == crypto && currentUser.Wallets.Contains(x.Id));
        }
    }
}
