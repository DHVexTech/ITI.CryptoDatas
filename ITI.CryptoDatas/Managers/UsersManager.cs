using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Managers
{
    public class UsersManager
    {
        private readonly IConfiguration _config;
        private readonly string _key; 

        public UsersManager(IConfiguration config)
        {
            _config = config;
            _key = _config["Token:Secret"];
        }

        public User GetUser(string username)
        {
            List<User> users = GetFromDatabase();
            if (users.Count > 0)
            {
                User user = users.First(x => x.Username == username);
                if (user != null)
                    return user;
            }
            return null;
        }

        public User Login(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return null;
            userInput.Password = EncryptePassword(userInput.Password);
            User user = GetUser(userInput.Username);
            if (user == null || user.Password != userInput.Password)
                return null;
            return Authenticate(user);
        }

        public User Register(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return null;
            List<User> users = GetFromDatabase();
            User user = users.First(x => x.Username == userInput.Username);
            if (users.First(x => x.Username == userInput.Username) != null)
                return null;
            userInput = Authenticate(user);
            users.Add(userInput);
            WriteInDatabase(users);
            return null;
        }

        private User Authenticate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }

        public bool Delete(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            List<User> users = GetFromDatabase();
            User user = users.First(x => x.Username == username);
            if (users.Remove(user))
            {
                WriteInDatabase(users);
                return true;
            }
            return false;
        }

        public bool Edit(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return false;
            List<User> users = GetFromDatabase();
            User user = users.First(x => x.Username == userInput.Username && x.Password == userInput.Password);
            if (user == null)
                return false;
            // TODO : remove wallet
            users.Remove(userInput);
            WriteInDatabase(users);
            return true;
        }



        private List<User> GetFromDatabase()
        {
            using (StreamReader r = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Databases\users.json")))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<User>>(json);
            }
        }

        private void WriteInDatabase(List<User> users)
        {
            using (StreamWriter w = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Databases\users.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, users);
            }
        }

        private string EncryptePassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }

    }
}
