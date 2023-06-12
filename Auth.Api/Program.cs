using Auth.BL;
using Auth.BL.Services;
using Auth.Common.Interfaces;
using Auth.DAL;
using Common.Configurators;
using Common.Configurators.ConfigClasses;
using delivery_backend_advanced.Services.ExceptionHandler;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProfilesService, ProfilesService>();
builder.Services.AddScoped<IAuthDbInitializer, AuthDbInitializer>();
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
builder.Services.AddHostedService<RabbitMqListener>();

var rabbitMqConnection = builder.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnection>();
builder.Services.AddSingleton<IConnection>(x =>
    new ConnectionFactory
    {
        HostName = rabbitMqConnection.Hostname,
        UserName = rabbitMqConnection.Username,
        Password = rabbitMqConnection.Password,
        VirtualHost = rabbitMqConnection.VirtualHost
    }.CreateConnection()
);

builder.ConfigureJwt();

builder.ConfigureIdentity();

builder.ConfigureAuthDAL();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureAuthDAL();

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();