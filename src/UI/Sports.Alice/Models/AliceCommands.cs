using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Alice.Models
{
    public static class AliceCommands
    {
        public const string LatestNews = "последние новости";
        public const string MainNews = "главные новости";
    }

    public enum AliceCommand
    {
        Undefined,
        LatestNews,
        MainNews
    }
}
