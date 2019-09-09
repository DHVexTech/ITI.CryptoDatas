using System.Threading.Tasks;
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
        private readonly IConfiguration _configuration;
        private readonly string _baseUri;
        private readonly string _apiKey;

        public CryptosController(IConfiguration config)
        {
            _cryptosManager = new CryptosManager();
            _configuration = config;
            _apiKey = _configuration.GetValue<string>("CoinMarket:Key");
            _baseUri = _configuration.GetValue<string>("CoinMarket:Sandbox");
        }

        [HttpGet("UpdateCryptoData/{crypto}")]
        public async Task<Crypto> GetCryptoData(string crypto) => await _cryptosManager.GetCryptoData(_baseUri, _apiKey, crypto);

        [HttpGet("/info/{crypto}")]
        public ActionResult<string> GetInfo(string crypto)
        {
            return "value";
        }

        [HttpGet]
        public ActionResult<string> Test()
        {
            return "value";
        }

    }
}