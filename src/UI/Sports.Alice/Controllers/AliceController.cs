using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sports.Alice.Services.Interfaces;
using Yandex.Alice.Sdk.Models;

namespace Sports.Alice.Controllers
{
    [ApiController]
    public class AliceController : ControllerBase
    {
        private readonly IAliceService _aliceService;
        private readonly ILogger<AliceController> _logger;

        public AliceController(IAliceService aliceService, ILogger<AliceController> logger)
        {
            _aliceService = aliceService;
            _logger = logger;
        }

        [HttpPost("/alice")]
        public IActionResult WebHook([FromBody] AliceRequest request)
        {
            _logger.LogInformation($"State is: {request.State}");
            var response = _aliceService.ProcessRequest(request);
            return Ok(response);
        }
    }
}
