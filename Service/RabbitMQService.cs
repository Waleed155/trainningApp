using RabbitMQ.Client;
using System.Text;

namespace trainningApp.Service
{
    public class RabbitMQService: IRabbitMQService ,IAsyncDisposable    {
        IConnection _connection;
        IChannel _channel;
        //public  RabbitMQService(IConnection connection,IChannel channel) {
        //    _connection = connection;
        //    _channel = channel;
        //    _channel.ExchangeDeclareAsync("Ex1", ExchangeType.Direct);
        //    _channel.QueueDeclareAsync("Q1", durable: true, autoDelete: false);
        //    _channel.QueueBindAsync("Q1", "Ex1", "key1");
            

        //}
        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        await    _channel.ExchangeDeclareAsync("Ex11", ExchangeType.Direct);
           await  _channel.QueueDeclareAsync("Q11", durable: true,exclusive:false, autoDelete: false);
            await   _channel.QueueBindAsync("Q11", "Ex11", "key11");
        }

   

        public void publishMessage(string message)
        {
            var body=Encoding.UTF8.GetBytes(message);
            _channel.BasicPublishAsync("Ex11","key11",body:body);
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
