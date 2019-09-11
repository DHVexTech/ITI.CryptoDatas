using System.Threading.Tasks;
using ITI.CryptoDatas.Helpers;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptosController : Controller
    {
        private readonly CryptosManager _cryptosManager;
        private readonly string _baseUri;
        private readonly string _apiKey;

        public CryptosController()
        {
            _cryptosManager = new CryptosManager();
            _apiKey = ConfigHelper.TokenSecret;
            _baseUri = ConfigHelper.CoinMarketUri;
        }

        [HttpGet("UpdateCryptoData/{crypto}")]
        public async Task<Crypto> GetCryptoData(string crypto) => await _cryptosManager.GetCryptoData(_baseUri, _apiKey, crypto);

    }
}