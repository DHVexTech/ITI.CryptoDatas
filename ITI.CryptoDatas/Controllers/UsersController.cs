using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        String url = "./users.json";
        [HttpGet]
        public ActionResult<bool> Login(string username, string password)
        {
            var data = JsonConvert.DeserializeObject<dataUser>(this.url);
            var users = data.Users;
            if(users.Count > 0)
            {
                if(users.FindIndex((User user) => user.username == username && user.password == password) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet]
        public ActionResult<string> Register(string username, string password)
        {
            var array = JArray.Parse(this.url);
            var itemToAdd = new JObject();
            itemToAdd["username"] = username;
            itemToAdd["password"] = "password";
            array.Add(itemToAdd);

            var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);
            System.IO.File.AppendAllText(this.url, jsonToOutput);

            return "value";
        }

        [HttpDelete]
        public ActionResult<string> Delete(string username)
        {
            var array = JArray.Parse(this.url);
            array.Remove();

            var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);
            System.IO.File.AppendAllText(this.url, jsonToOutput);

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