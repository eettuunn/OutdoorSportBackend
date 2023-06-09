using Auth.DAL;
using Common.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();