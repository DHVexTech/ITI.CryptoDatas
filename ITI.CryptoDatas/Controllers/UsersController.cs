using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            _userManager = usersManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<User> Login([FromBody]User userData)
        {
           if (string.IsNullOrEmpty(userData.Username) || string.IsNullOrEmpty(userData.Password)) return null;
           return _userManager.Login(userData);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<User> Register([FromBody]User userData)
        {
            if (string.IsNullOrEmpty(userData.Username) || string.IsNullOrEmpty(userData.Password)) return null;
            return _userManager.Register(userData);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<bool> Delete(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return _userManager.Delete(username);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<bool> Edit(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return false;
            return _userManager.Edit(userInput);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<User> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            return _userManager.GetUser(username);
        }
    }
}