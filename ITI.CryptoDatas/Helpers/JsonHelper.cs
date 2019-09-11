using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Helpers
{
    public static class JsonHelper
    {
        public static List<T> GetFromDatabase<T>(string filename)
        {
            using (StreamReader r = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Databases\" + filename + ".json").Replace(".Tests", "")))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }

        public static void WriteInDatabase<T>(List<T> users, string filename)
        {
            using (StreamWriter w = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Databases\" + filename + ".json").Replace(".Tests", "")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, users);
            }
        }
    }
}
