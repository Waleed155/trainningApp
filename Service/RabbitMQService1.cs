using RabbitMQ.Client;
using System.Text;

namespace trainningApp.Service
{
    public class RabbitMQService1:IRabbitMQService1,IAsyncDisposable
    {
        IConnection _connection;
        IChannel _channel;
        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _channel= await _connection.CreateChannelAsync();
           await _channel.ExchangeDeclareAsync("Ex111", type: ExchangeType.Direct);
            await _channel.QueueDeclareAsync("Q111", durable: true, exclusive: false, autoDelete: false);
       await _channel.QueueBindAsync("Q111", "Ex111", "key111");
                } 
        public async Task  publishMessage(string message)
        {
            var body =Encoding.UTF8.GetBytes(message);
           await _channel.BasicPublishAsync("Ex111", "key111", body: body);
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
