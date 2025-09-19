
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using trainningApp.Models;
using trainningApp.ViewModel;

namespace trainningApp.Service
{
    public class RabbitMQConsumerService : IHostedService ,IAsyncDisposable
    {
          IConnection _connection;
        IChannel _channel;
    
        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync +=  Consumer_ReceivedAsync;
          await  _channel.BasicConsumeAsync("Q11", autoAck:false,consumer: consumer);
        }

        private async Task Consumer_ReceivedAsync(object? sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var message = Encoding.UTF8.GetString(@event.Body.ToArray());
                var baseMessage=Newtonsoft.
                    Json.
                    JsonConvert.
                    DeserializeObject<RabbitMQBaseMessage>(message);
                switch (baseMessage.messageType)
                {
                    case  messageType.Department:
                        
                            var department = Newtonsoft.
                            Json.
                            JsonConvert.
                            DeserializeObject<DepartmentViewModel>(message);
                        
                        break;
                    case messageType.Employee:

                        var employee = Newtonsoft.
                        Json.
                        JsonConvert.
                        DeserializeObject<Employee>(message);

                        break;

                }
                await _channel.BasicAckAsync(@event.DeliveryTag, multiple: false);

            }
            catch
            {
              await  _channel.BasicRejectAsync(@event.DeliveryTag,requeue:true);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}
