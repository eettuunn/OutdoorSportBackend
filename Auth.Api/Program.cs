using Auth.BL;
using Auth.BL.Services;
using Auth.Common.Interfaces;
using Auth.DAL;
using Common.Configurators;
using Common.Configurators.ConfigClasses;
using Common.Policies.Ban;
using delivery_backend_advanced.Services.ExceptionHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:63343");
    });
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:63342");
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProfilesService, ProfilesService>();
builder.Services.AddScoped<IAuthDbInitializer, AuthDbInitializer>();
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));

var rabbitMqConnection = builder.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnection>();
builder.Services.AddSingleton<IConnection>(x =>
    new ConnectionFactory
    {
        HostName = rabbitMqConnection.Hostname,
        UserName = rabbitMqConnection.Username,
        Password = rabbitMqConnection.Password,
        VirtualHost = rabbitMqConnection.VirtualHost,
        Port = int.Parse(rabbitMqConnection.Port),
        Ssl =
        {
            ServerName = rabbitMqConnection.Hostname,
            Enabled = false
        }
    }.CreateConnection()
);
builder.Services.AddHostedService<RabbitMqListener>();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, BanPolicyHandler>();

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

app.UseCors();

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();