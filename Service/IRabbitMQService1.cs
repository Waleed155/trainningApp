using RabbitMQ.Client;
using System.Text;

namespace trainningApp.Service
{
    public interface IRabbitMQService1
    {
        public  Task InitializeAsync();

        public Task publishMessage(string message);
       
    }
}
