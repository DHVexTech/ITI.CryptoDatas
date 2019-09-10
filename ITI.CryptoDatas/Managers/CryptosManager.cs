using ITI.CryptoDatas.Enums;
using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Models;
using ITI.CryptoDatas.Models.MarketCoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Managers
{
    public class CryptosManager
    {
        private readonly string _databaseName;
        public CryptosManager()
        {
            _databaseName = "cryptos";
        }

        public async Task<Crypto> GetCryptoData(string baseUri, string apiKey, string crypto)
        {
            string cryptoName = crypto.ToUpper();
            Enum.TryParse(cryptoName, out CryptoEnum cryptoSelected);
            if (cryptoSelected == CryptoEnum.None)
                throw new ArgumentException("The current crypto is not exist or not supported");

            string response = await HttpHelper.SendRequest(baseUri, "v1/cryptocurrency/listings/latest", apiKey);
            Price priceResult = JsonConvert.DeserializeObject<Price>(response);
            CryptoCurrency cryptoResult = priceResult.Data.First(x => x.Id == (int)cryptoSelected);

            List<Crypto> cryptos = JsonHelper.GetFromDatabase<Crypto>(_databaseName);
            Crypto cryptoModel = cryptos.Find(x => x.Name == cryptoName);
            if (cryptoModel != null)
                cryptos.Remove(cryptoModel);

            cryptoModel = new Crypto()
            {
                LastUpdate = DateTime.UtcNow,
                Name = cryptoName,
                CryptoPrice = cryptoResult.Quote.USD.Price
            };

            cryptos.Add(cryptoModel);
            JsonHelper.WriteInDatabase<Crypto>(cryptos, _databaseName);
            return cryptoModel;
        }
    }
}
