
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using trainningApp.ViewModel;

namespace trainningApp.Service
{
    public class RabbitMQConsumerService1 : IHostedService,IAsyncDisposable
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
            consumer.ReceivedAsync += consumer_Received;
            await _channel.BasicConsumeAsync("Q111", autoAck: false, consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private RabbitMQBaseMessage1 GetBaseMessage1(string message)
        {
            var jsonObj = Newtonsoft.Json.Linq.JObject.Parse(message);
            var typeName = jsonObj["type"].ToString();
            string nameSpace = "RabbitMqMessages";

            Type type = Type.GetType($"{nameSpace}.{typeName},trainningApp");
            if (type is null)
            {
                throw new Exception("error");
            }
            else
            {
                var baseMessage = Newtonsoft.
                    Json.
                    JsonConvert.
                    DeserializeObject(message, type) as RabbitMQBaseMessage1;
                baseMessage.type = typeName.Replace("Message", "Consumer");
                return baseMessage;
            }
        }
        private async Task consumer_Received(object?sender,BasicDeliverEventArgs e)
        {
            try
            {
                var message=Encoding.UTF8.GetString(e.Body.ToArray());
                var baseMessage=GetBaseMessage1(message);
                InvokeConsumer(baseMessage);
              await  _channel.BasicAckAsync(e.DeliveryTag, multiple: false);
            }
            catch
            {
              await  _channel.BasicRejectAsync(e.DeliveryTag,requeue: false);
            }
        }
        private  void InvokeConsumer(RabbitMQBaseMessage1 baseMessage1)
        {
            var consumerType = Type.GetType($"ConsumerMessages.{baseMessage1.type},trainningApp");
            if (consumerType == null) 
            {
                throw new Exception("error"); 
            }
            else
            {
               var consumer=Activator.CreateInstance(consumerType);
                var method = consumerType.GetMethod("consume");
                 method.Invoke(consumer, new object[] {baseMessage1});

            }
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
