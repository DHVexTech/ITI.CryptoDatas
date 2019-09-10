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

        /*public Wallet Edit(Wallet walletInput)
        {
            List<Wallet> wallets = GetFromDatabase();
            Wallet wallet = wallets.First(x => x.id == walletInput.id);
            if (wallet == null)
                return null;

        }*/



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
