using Microsoft.AspNetCore.Mvc;
using Sports.Alice.Services.Interfaces;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Controllers
{
    [ApiController]
    public class AliceController : ControllerBase
    {
        private readonly IAliceService _aliceService;

        public AliceController(IAliceService aliceService)
        {
            _aliceService = aliceService;
        }

        [HttpPost("/alice")]
        public AliceResponse WebHook([FromBody] AliceRequest request)
        {
            return _aliceService.ProcessRequest(request);
        }
    }
}
