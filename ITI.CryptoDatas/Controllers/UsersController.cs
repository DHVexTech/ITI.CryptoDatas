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
            if (string.IsNullOrEmpty(userData.Username) || string.IsNullOrEmpty(userData.Password)) return Forbid();
            User user = _userManager.Login(userData);
            if (user == null)
                return Forbid();
            else
                return Json(user);
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody]User userData)
        {
            if (string.IsNullOrEmpty(userData.Username) || string.IsNullOrEmpty(userData.Password)) return Forbid();
            bool result = _userManager.Register(userData);
            if (result)
                return Ok();
            else
                return UnprocessableEntity();
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<bool> Delete(string username)
        {
            if (string.IsNullOrEmpty(username)) return Forbid();
            return _userManager.Delete(username);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<bool> Edit(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return Forbid();
            return _userManager.Edit(userInput);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<User> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return Forbid();
            return _userManager.GetUser(username);
        }
    }
}