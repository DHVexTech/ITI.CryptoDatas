using System.Linq;
using System.Security.Claims;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly TransactionsManager _transactionManager;
        private object _walletManager;

        public TransactionsController(TransactionsManager transactionsManager)
        {
            _transactionManager = transactionsManager;
        }

        [HttpPost("removefund")]
        [Authorize]
        public ActionResult<Transaction> Give([FromBody]Transaction transaction)
        {
            ClaimsIdentity currentUsername = HttpContext.User.Identities.First(x => x.Name != null);
            return _transactionManager.Give(currentUsername.Name, transaction);
        }

    }
}