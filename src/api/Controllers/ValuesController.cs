using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ValuesProducerService _valuesProducerService;

        public ValuesController(IHostedService hostedService)
        {
            _valuesProducerService = hostedService as ValuesProducerService;
        }

        [HttpGet("start")]
        public ActionResult Start()
        {
            _ = _valuesProducerService.StartAsync(new System.Threading.CancellationToken());

            return Ok();
        }

        [HttpGet("stop")]
        public ActionResult Stop()
        {
            _ = _valuesProducerService.StopAsync(new System.Threading.CancellationToken());

            return Ok();
        }
    }
}
