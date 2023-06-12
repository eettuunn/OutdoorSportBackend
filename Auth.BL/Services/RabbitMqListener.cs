using System.Text;
using Auth.DAL;
using Backend.Common.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.BL.Services;

public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;

    public RabbitMqListener(IConnection connection, IServiceProvider serviceProvider)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
        _channel = _connection.CreateModel();
    }

    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        _channel.QueueDeclare("reports", durable: true, exclusive: false);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, eventArgs) => BanUser(eventArgs, stoppingToken);

        _channel.BasicConsume("reports", true, consumer);
        
        return Task.CompletedTask;
    }
    
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }



    private async Task BanUser(BasicDeliverEventArgs eventArgs, CancellationToken stoppingToken)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var banModel = JsonConvert.DeserializeObject<BanUserModel>(message);

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            
            var user = await context
                .Users
                .FirstOrDefaultAsync(u => u.Email == banModel.email, cancellationToken: stoppingToken);
            user.IsBanned = banModel.ban;
            
            await context.SaveChangesAsync(stoppingToken);
        }
    }
}