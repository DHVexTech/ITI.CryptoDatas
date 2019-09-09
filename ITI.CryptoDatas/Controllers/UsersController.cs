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
        public ActionResult<bool> Register(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return false;
            var array = JArray.Parse(this.url);
            var itemToAdd = new JObject();
            itemToAdd["username"] = username;
            itemToAdd["password"] = password;
            array.Add(itemToAdd);

            var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);
            System.IO.File.AppendAllText(this.url, jsonToOutput);
            return true;
        }

        [HttpDelete]
        public ActionResult<bool> Delete(string username)
        {
            if (String.IsNullOrEmpty(username)) return false;
            var array = JArray.Parse(this.url);
            var item = this.ToFind(username);
            array.Remove(item.Value);
            var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);
            System.IO.File.AppendAllText(this.url, jsonToOutput);
            return true;
        }

        [HttpPut]
        public ActionResult<bool> Edit(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return false;
            var array = JArray.Parse(this.url);
            var item = this.ToFind(username);
            array.Remove(item.Value);
            var itemToAdd = new JObject();
            itemToAdd["username"] = username;
            itemToAdd["password"] = password;
            array.Add(itemToAdd);
            var jsonToOutput = JsonConvert.SerializeObject(array, Formatting.Indented);
            System.IO.File.AppendAllText(this.url, jsonToOutput);
            return true;
        }

        [HttpPost]
        public ActionResult<JObject> ToFind(string username)
        {
            var array = JArray.Parse(this.url);
            var item = array.Children<JObject>().FirstOrDefault(o => o["username"] != null && o["username"].ToString() == username);
            return item;
        }
    }
}