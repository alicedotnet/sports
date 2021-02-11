using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Models
{
    public class SportsButtonModel : AliceButtonModel
    {
        public SportsButtonModel(string title)
            : base(title, true)
        {

        }
    }
}
