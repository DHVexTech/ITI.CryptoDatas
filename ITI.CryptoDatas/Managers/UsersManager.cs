using ITI.CryptoDatas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Managers
{
    public class UsersManager
    {
        public UsersManager()
        {
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
            User user = GetUser(userInput.Username);
            if (user == null || user.Password != userInput.Password)
                return null;
            return user;
        }

        public bool Register(User userInput)
        {
            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password)) return false;
            List<User> users = GetFromDatabase();
            if (users.First(x => x.Username == userInput.Username) != null)
                return false;
            users.Add(userInput);
            WriteInDatabase(users);
            return true;
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
    }
}
