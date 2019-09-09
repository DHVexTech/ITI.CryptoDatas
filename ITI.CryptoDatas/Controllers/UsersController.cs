using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ITI.CryptoDatas.Managers;
using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ITI.CryptoDatas.Controllers
{
    interface dataUser
    {
        List<User> Users { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        string _url;
        private UsersManager _userManager;

        public UsersController()
        {
            _userManager = new UsersManager();
            _url = "./users.json";
        }


        [HttpPost("login")]
        public ActionResult<User> Login([FromBody]User userData)
        {
           if (string.IsNullOrEmpty(userData.Username) || string.IsNullOrEmpty(userData.Password)) return null;
           return _userManager.Login(userData);
        }

        [HttpPost("register")]
        public ActionResult<bool> Register([FromBody]User userData)
        {
            if (string.IsNullOrEmpty(userData.Username) || string.IsNullOrEmpty(userData.Password)) return null;
            return _userManager.Register(userData);
        }

        [HttpDelete]
        public ActionResult<bool> Delete(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return _userManager.Delete(username);
        }

        //[HttpPut]
        //public ActionResult<bool> Edit(string username, string password)
        //{
        //    if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return false;
        //    var array = JArray.Parse(this.url);
        //    var item = this.ToFind(username);
        //    array.Remove(item.Value);
        //    var itemToAdd = new JObject();
        //    itemToAdd["username"] = username;
        //    itemToAdd["password"] = password;
        //    array.Add(itemToAdd);
        //    var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);
        //    System.IO.File.AppendAllText(this.url, jsonToOutput);
        //    return true;
        //}

        [HttpGet]
        public ActionResult<User> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            return _userManager.GetUser(username);
        }
    }
}