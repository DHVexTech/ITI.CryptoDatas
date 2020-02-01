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
        private UsersManager _usersManager;

        public WalletsController(WalletsManager walletsManager, UsersManager usersManager)
        {
        }

        [HttpPost]
        public ActionResult<bool> Add([FromBody]Wallet wallet)
        {
            throw new NotImplementedException();
        }

        [HttpPut("refund")]
        [Authorize]
        public ActionResult<Wallet> Refund([FromBody]Wallet wallet)
        {
            throw new NotImplementedException();
        }

        [HttpPut("removefund")]
        [Authorize]
        public ActionResult<Wallet> RemoveFund([FromBody]Wallet wallet)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Wallet>> GetUserWallets()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{crypto}")]
        [Authorize]
        public ActionResult<List<Wallet>> GetSpecificWallet(string crypto)
        {
            throw new NotImplementedException();
        }
    }
}