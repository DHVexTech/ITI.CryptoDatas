using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult<string> Login()
        {
            return "value";
        }

        [HttpGet]
        public ActionResult<string> Register()
        {
            return "value";
        }

        [HttpDelete]
        public ActionResult<string> Delete()
        {
            return "value";
        }

        [HttpPut]
        public ActionResult<string> Edit()
        {
            return "value";
        }

        // find an implmentation
        [HttpPost]
        public ActionResult<string> ToFind()
        {
            return "value";
        }
    }
}