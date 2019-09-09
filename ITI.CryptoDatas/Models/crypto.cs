using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Models
{
    public class Crypto
    {
        public string Name { get; set; }
        public double CryptoPrice { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
