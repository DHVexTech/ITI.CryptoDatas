using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UsernameGiver { get; set; }
        public string UsernameReceiver { get; set; }
        public double Fund { get; set; }
        public string Crypto { get; set; }
    }
}
