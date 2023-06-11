using System.Text.Json.Serialization;
using Backend.BL;
using Backend.BL.Services;
using Backend.Common.Interfaces;
using Common.Configurators;
using OutdoorSportBackend.Configurators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBackDbInitializer, BackDbInitializer>();
builder.Services.AddScoped<IObjectsService, ObjectsService>();
builder.Services.AddAutoMapper(typeof(BackMappingProfile));

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.ConfigureBackendDAL();

builder.ConfigureJwt();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureBackendDAL();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();