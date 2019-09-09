using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CryptoDatas.Models.MarketCoin
{
    public class Price
    {
        public Status Status { get; set; }
        public List<CryptoCurrency> Data { get; set; }
    }

    public class Status
    {
        public DateTime Timestamp { get; set; }
        public int Error_code { get; set; }
        public object Error_message { get; set; }
        public int Elapsed { get; set; }
        public int Credit_count { get; set; }
    }

    public class CryptoCurrency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public int Num_market_pairs { get; set; }
        public DateTime Date_added { get; set; }
        public List<string> Tags { get; set; }
        public float? Max_supply { get; set; }
        public float Circulating_supply { get; set; }
        public float Total_supply { get; set; }
        public Platform Platform { get; set; }
        public int Cmc_rank { get; set; }
        public DateTime Last_updated { get; set; }
        public Quote Quote { get; set; }
    }

    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public string Token_address { get; set; }
    }

    public class Quote
    {
        public USD USD { get; set; }
    }

    public class USD
    {
        public float Price { get; set; }
        public float Volume_24h { get; set; }
        public float Percent_change_1h { get; set; }
        public float Percent_change_24h { get; set; }
        public float Percent_change_7d { get; set; }
        public float Market_cap { get; set; }
        public DateTime Last_updated { get; set; }
    }

}

