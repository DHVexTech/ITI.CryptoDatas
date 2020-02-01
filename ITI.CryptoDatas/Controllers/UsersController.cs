using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ITI.CryptoDatas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private UsersManager _userManager;

        public UsersController(UsersManager usersManager)
        {
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<User> Login([FromBody]User userData)
        {
            throw new NotImplementedException();
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody]User userData)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<bool> Delete()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Authorize]
        public ActionResult<bool> Edit(User userInput)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<User> GetUser(string username)
        {
            throw new NotImplementedException();
        }
    }
}