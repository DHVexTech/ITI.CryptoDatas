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
        public Wallet GetWallet(int walletId)
        {
            List<Wallet> wallets = GetFromDatabase();
            if (wallets.Count > 0)
            {
                Wallet wallet = wallets.First(x => x.id == walletId);
                if (wallet != null)
                    return wallet;
            }
            return null;
        }

        public bool Add (Wallet wallet) 
        {
            List<Wallet> list = GetFromDatabase();
            list.Add(wallet);
            WriteInDatabase(list);
            return true;
        }

        public bool Delete(int walletId)
        {
            List<Wallet> wallets = GetFromDatabase();
            Wallet wallet = wallets.First(x => x.id == walletId);
            if (wallets.Remove(wallet))
            {
                WriteInDatabase(wallets);
                return true;
            }
            return false;
        }

        private List<Wallet> GetFromDatabase()
        {
            using (StreamReader r = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Databases\wallets.json")))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Wallet>>(json);
            }
        }

        private void WriteInDatabase(List<Wallet> wallets)
        {
            using (StreamWriter w = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Databases\wallets.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, wallets);
            }
        }
    }
}
