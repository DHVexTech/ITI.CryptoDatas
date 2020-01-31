using ITI.CryptoDatas.Enums;
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
        private readonly string _key; 
        private readonly string _databaseName;
        private readonly WalletsManager _walletsManager;

        public UsersManager(WalletsManager walletsManager)
        {
            _key = ConfigHelper.TokenSecret;
            _databaseName = "users";
            _walletsManager = walletsManager;
        }

        public User GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public User Login(User userInput)
        {
            throw new NotImplementedException();

        }

        public User Register(User userInput)
        {
            throw new NotImplementedException();
        }

        private User Authenticate(User user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string username)
        {
            throw new NotImplementedException();
        }

        public bool Edit(User userInput)
        {
            throw new NotImplementedException();
        }
    }
}
