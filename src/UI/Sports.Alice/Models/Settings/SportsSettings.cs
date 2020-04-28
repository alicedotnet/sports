using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Alice.Models.Settings
{
    public class SportsSettings
    {
        public int NewsToDisplay { get; set; }
        public int CommentsToDisplay { get; set; }
        public int DaysToKeepData { get; set; }
    }
}
