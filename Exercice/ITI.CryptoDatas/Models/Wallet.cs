using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string CryptoName { get; set; }
        public double Fund { get; set; }
    }
}