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
            User result = _userManager.Register(userData);
            if (result != null)
                return Json(result);
            else
                return UnprocessableEntity();
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<bool> Delete()
        {
            ClaimsIdentity currentUsername = HttpContext.User.Identities.First(x => x.Name != null);
            return _userManager.Delete(currentUsername.Name);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<bool> Edit(User userInput)
        {
            ClaimsIdentity currentUsername = HttpContext.User.Identities.First(x => x.Name != null);
            if (userInput.Username != currentUsername.Name) return false;
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