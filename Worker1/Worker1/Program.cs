
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using SharedTypes.Messages;

namespace Worker1
{
    public class Program
    {
        private const string userName = "guest";
        private const string userPassword = "guest";
        private const string connectionUrl = "host.docker.internal";
        private const string connectionPort = "5672";
        private const string channel = "myBus";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var connectionString = CreateMqConnectionString(userName, userPassword, connectionUrl, connectionPort);

            using var activator = new BuiltinHandlerActivator();
            activator.Register(() => new RequestHandler(activator));

            var subscriber = Configure.With(activator)
                .Transport(t => t.UseRabbitMq(connectionString, channel))
                //.Routing(r => r.TypeBased().MapAssemblyOf<Request>("publisher")) //This needs to be correct somehow
                .Start();
            
            subscriber.Subscribe<Request>().Wait();

            app.Run();
        }

        private static string CreateMqConnectionString(string userName, string userPassword, string connectionUrl, string connectionPort)
        {
            return $"amqp://{userName}:{userPassword}@{connectionUrl}:{connectionPort}/";
        }
    }
}
