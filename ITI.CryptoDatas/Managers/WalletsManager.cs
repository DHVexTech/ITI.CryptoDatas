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
            List<int> list = new List<int>();
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            int id = wallets.Count != 0 ? wallets.Last().Id + 1: 0;
            foreach (var item in Enum.GetNames(typeof(CryptoEnum)))
                if (item != "None")
                {
                    Wallet currentWallet = new Wallet();
                    currentWallet.CryptoName = item;
                    currentWallet.Fund = 0;
                    currentWallet.Id = id++;
                    Add(currentWallet);
                    list.Add(currentWallet.Id);
                }
            return list;
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

        public Wallet ManageFund(Wallet refundProps, User currentUser, string @operator)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            Wallet walletSelected = wallets.Single(x => x.CryptoName == refundProps.CryptoName && currentUser.Wallets.Contains(x.Id));
            if (@operator == "+")
                walletSelected.Fund += refundProps.Fund;
            else
                walletSelected.Fund -= refundProps.Fund;
            JsonHelper.WriteInDatabase<Wallet>(wallets, _databaseName);
            //walletSelected.Id = 0;
            return walletSelected;
        }

        public List<Wallet> GetUserWallets(User user)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            return wallets.Where(x => user.Wallets.Contains(x.Id)).ToList();
        }

        public Wallet GetSpecificWallet(User user, string crypto)
        {
            List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
            return wallets.First(x => user.Wallets.Contains(x.Id) && x.CryptoName == crypto);
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

        //private Wallet GetUserWallet(string username, string crypto)
        //{
        //    List<Wallet> wallets = JsonHelper.GetFromDatabase<Wallet>(_databaseName);
        //    User currentUser = _userManager.GetUser(username);
        //    return wallets.Single(x => x.CryptoName == crypto && currentUser.Wallets.Contains(x.Id));
        //}
    }
}
