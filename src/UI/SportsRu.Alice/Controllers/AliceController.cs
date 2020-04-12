using Microsoft.AspNetCore.Mvc;
using Yandex.Alice.Sdk.Models;

namespace SportsRu.Alice.Controllers
{
    [ApiController]
    public class AliceController : ControllerBase
    {
        [HttpPost("/alice")]
        public AliceResponse WebHook([FromBody] AliceRequest request)
        {
            return new AliceResponse()
            {
                Session = request.Session,
                Version = request.Version,
                Response = new AliceResponseModel()
                {
                    Text = "Hello World!"
                }
            };
        }
    }
}
