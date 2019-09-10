using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : Controller
    {
        private WalletsManager _walletManager;

        public WalletsController()
        {
            _walletManager = new WalletsManager();
        }

        [HttpPost]
        public ActionResult<bool> Add([FromBody]Wallet wallet)
        {
            return _walletManager.Add(wallet);
        }
    }
}