using Microsoft.AspNetCore.Mvc;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Messages;
using SharedTypes.Messages;
using System.Net.NetworkInformation;

namespace API_Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IBus _bus;

        public DataController(Serilog.ILogger logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet("rabbitmq")]
        public async Task<ActionResult> UseRabbitMq()
        {
            _logger.Debug("Called {0}", nameof(UseRabbitMq));

            var messageId = Guid.NewGuid().ToString();
            var header = new Dictionary<string, string>
            {
                { "rbs2-corr-id", Guid.NewGuid().ToString() },
                { "rbs2-msg-id", messageId },
                { "rbs2-senttime", DateTime.Now.ToString() },
            };

            var messageToPass = new Request(header, "This request");

            using var activator = new BuiltinHandlerActivator();
            activator.Handle<Response>(async message =>
            {
                _logger.Information("Got Response back with message {0}", message.Result);
            });

            //Await data returned
            await _bus.Subscribe<Response>();

            //Send data
            await _bus.Publish(messageToPass, header);

            _logger.Debug("Completed {0}", nameof(UseRabbitMq));
            return Ok("Call successful!");
        }
    }
}
