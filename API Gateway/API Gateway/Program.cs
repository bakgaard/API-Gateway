using Rebus.Config;
using Rebus.Retry.Simple;
using Serilog;

namespace API_Gateway
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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateBootstrapLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = CreateMqConnectionString(userName, userPassword, connectionUrl, connectionPort);

            builder.Services.AddRebus(conf => conf
                .Logging(l => l.Serilog(Log.Logger))
                .Options(o => o.SimpleRetryStrategy(maxDeliveryAttempts: 3))
                .Transport(t => t.UseRabbitMq(connectionString, channel))
            );

            var app = builder.Build();
            //app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static string CreateMqConnectionString(string userName, string userPassword, string connectionUrl, string connectionPort)
        {
            return $"amqp://{userName}:{userPassword}@{connectionUrl}:{connectionPort}/";
        }
    }
}