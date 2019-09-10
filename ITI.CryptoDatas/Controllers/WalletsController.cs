using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : Controller
    {
        private WalletsManager _walletManager;

        public WalletsController(WalletsManager walletsManager)
        {
            _walletManager = walletsManager;
        }

        [HttpPost]
        public ActionResult<bool> Add([FromBody]Wallet wallet)
        {
            return _walletManager.Add(wallet);
        }

        [HttpPut("refund")]
        [Authorize]
        public ActionResult<Wallet> Refund([FromBody]Wallet wallet)
        {
            ClaimsIdentity currentUsername = HttpContext.User.Identities.First(x => x.Name != null);
            return _walletManager.Refund(wallet, currentUsername.Name);
        }
    }
}