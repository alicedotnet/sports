using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Services.Interfaces
{
    public interface IAliceService
    {
        AliceResponse ProcessRequest(AliceRequest aliceRequest);
    }
}
