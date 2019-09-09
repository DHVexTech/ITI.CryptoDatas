using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CryptoDatas.Managers;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptosController : Controller
    {
        private readonly CryptosManager _cryptosManager;

        public CryptosController()
        {
            _cryptosManager = new CryptosManager();
        }

        [HttpGet]
        public ActionResult<string> Login()
        {
            return "value";
        }

    }
}