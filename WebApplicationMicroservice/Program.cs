using Marten;
using Carter;
using System.Reflection;
using ServiceStack.Redis;
using System.Security.Authentication;
using System.Xml.Xsl;
using MongoDB.Driver;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>
{
    options.Connection("Host=localhost;Port=5432;Username=dbuser;Password=password123;Database=mydatabase;");

});

var connectionString = builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString");
var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
builder.Services.AddSingleton<IMongoClient>(new MongoClient(settings));

builder.Services.AddSingleton<IRedisClientsManager>(new RedisManagerPool("localhost:6379"));

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(Assembly).Assembly);
});

builder.Services.AddCarter();

// Add MassTransit and configure RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Configure RabbitMQ settings
        cfg.Host("rabbitmq://localhost"); // URL of your RabbitMQ server

        // Optionally configure additional settings (e.g., queues, timeouts, etc.)
        cfg.ConfigureEndpoints(context); // Automatically configure endpoints for consumers
    });
});

builder.Services.AddMassTransitHostedService(); // To manage lifecycle of MassTransit

var app = builder.Build();

app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.Run();
