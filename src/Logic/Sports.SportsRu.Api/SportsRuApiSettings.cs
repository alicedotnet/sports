using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sports.SportsRu.Api
{
    public class SportsRuApiSettings
    {
        public SportsRuApiSettings()
        {
        }

        public SportsRuApiSettings(string sportsRuBaseAddress, string sportsRuStatBaseAddress)
        {
            SportsRuBaseAddress = new Uri(sportsRuBaseAddress);
            SportsRuStatBaseAddress = new Uri(sportsRuStatBaseAddress);
        }

        public Uri SportsRuBaseAddress { get; set; }
        public Uri SportsRuStatBaseAddress { get; set; }
    }
}
