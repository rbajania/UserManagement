using UserService.Data;
using UserService.Services;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using MassTransit;
using UserService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient<UserServiceHttpClient>().AddPolicyHandler(GetPolicy());

builder.Services.AddDbContext<MongoDBContext>(options =>
{
    var connectionstring = builder.Configuration.GetConnectionString("MongoDbConnection");
    var database = "USERDB";
    options.UseMongoDB(connectionstring, database);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<UserCreatedConsumer>();
    x.AddConsumersFromNamespaceContaining<UserUpdatedConsumer>();
    x.AddConsumersFromNamespaceContaining<UserDeletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
