using Sports.Alice.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Services
{
    public class AliceService : IAliceService
    {
        public AliceResponse ProcessRequest(AliceRequest aliceRequest)
        {
            if(aliceRequest == null)
            {
                throw new ArgumentNullException(nameof(aliceRequest));
            }

            return new AliceResponse()
            {
                Session = aliceRequest.Session,
                Version = aliceRequest.Version,
                Response = new AliceResponseModel()
                {
                    
                }
            };
        }
    }
}
