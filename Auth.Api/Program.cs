using Auth.BL;
using Auth.BL.Services;
using Auth.Common.Dtos;
using Auth.Common.Interfaces;
using Auth.DAL;
using Common.Configurators;
using delivery_backend_advanced.Services.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));

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