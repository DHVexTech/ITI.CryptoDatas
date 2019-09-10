using ITI.CryptoDatas.Helpers;
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
using System.Threading;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Managers
{
    public class UsersManager
    {
        private readonly IConfiguration _config;
        private readonly string _key; 
        private readonly string _databaseName;

        public UsersManager(IConfiguration config)
        {
            _config = config;
            _key = _config["Token:Secret"];
            _databaseName = "users";
        }

        public User GetUser(string username)
        {
            List<User> users = JsonHelper.GetFromDatabase<User>(_databaseName);
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
            userInput.Password = EncryptionHelper.EncryptePassword(userInput.Password);
            User user = GetUser(userInput.Username);
            if (user == null || user.Password != userInput.Password)
                return null;
            return Authenticate(user);
        }

        public User Register(User userInput)
        {
            List<User> users = JsonHelper.GetFromDatabase<User>(_databaseName);
            User user = users.FirstOrDefault(x => x.Username == userInput.Username);
            if (user != null)
                return null;
            userInput = Authenticate(userInput);
            userInput.Password = EncryptionHelper.EncryptePassword(userInput.Password);
            users.Add(userInput);
            // TODO : Add wallet
            JsonHelper.WriteInDatabase<User>(users, _databaseName);
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
            List<User> users = JsonHelper.GetFromDatabase<User>(_databaseName);
            User user = users.First(x => x.Username == username);
            if (users.Remove(user))
            {
                JsonHelper.WriteInDatabase<User>(users, _databaseName);
                return true;
            }
            return false;
        }

        public bool Edit(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return false;
            userInput.Password = EncryptionHelper.EncryptePassword(userInput.Password);
            List<User> users = JsonHelper.GetFromDatabase<User>(_databaseName);
            User user = users.First(x => x.Username == userInput.Username && x.Password == userInput.Password);
            if (user == null)
                return false;
            // TODO : remove wallet
            users.Remove(userInput);
            JsonHelper.WriteInDatabase<User>(users, _databaseName);
            return true;
        }
    }
}
