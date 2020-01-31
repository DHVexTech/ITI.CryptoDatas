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
            _usersManager = usersManager;
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
            User user = _usersManager.GetUser(currentUsername.Name);
            return Json(_walletManager.ManageFund(wallet, user, "+"));
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
            ClaimsIdentity currentUsername = HttpContext.User.Identities.First(x => x.Name != null);
            User user = _usersManager.GetUser(currentUsername.Name);
            return Json(_walletManager.GetUserWallets(user));
        }

        [HttpGet("{crypto}")]
        [Authorize]
        public ActionResult<List<Wallet>> GetSpecificWallet(string crypto)
        {
            ClaimsIdentity currentUsername = HttpContext.User.Identities.First(x => x.Name != null);
            User user = _usersManager.GetUser(currentUsername.Name);
            return Json(_walletManager.GetSpecificWallet(user, crypto));
        }
    }
}