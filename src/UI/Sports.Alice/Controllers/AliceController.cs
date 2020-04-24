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
        public IActionResult WebHook([FromBody] AliceRequest request)
        {
            var response = _aliceService.ProcessRequest(request);
            return Ok(response);
        }
    }
}
