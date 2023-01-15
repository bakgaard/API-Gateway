using Rebus.Activation;
using Rebus.Handlers;
using Rebus.Messages;
using SharedTypes.Messages;
using System.Text.Json;

namespace Worker1
{
    public class RequestHandler : IHandleMessages<Request>
    {
        private BuiltinHandlerActivator activator;

        public RequestHandler(BuiltinHandlerActivator activator)
        {
            this.activator = activator;
        }

        public Task Handle(Request message)
        {
            var messageId = Guid.NewGuid().ToString();
            var headers = new Dictionary<string, string>
            {
                { "rbs2-corr-id", message.Headers["rbs2-msg-id"] }, //Attach to whom we respond to
                { "rbs2-msg-id", messageId },
                { "rbs2-senttime", DateTime.Now.ToString() },
            };

            var jsonData = JsonSerializer.Serialize("myMessage");
            var response = new Response(headers, jsonData);

            return activator.Bus.Publish(response);
        }
    }
}