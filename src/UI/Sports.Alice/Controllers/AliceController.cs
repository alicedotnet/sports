using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sports.Alice.Models;
using Sports.Alice.Services.Interfaces;

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
        public IActionResult WebHook([FromBody] SportsRequest request)
        {
            var response = _aliceService.ProcessRequest(request);
            return Ok(response);
        }
    }
}
