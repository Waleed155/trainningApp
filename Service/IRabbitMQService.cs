using System.Text;

namespace trainningApp.Service
{
    public interface IRabbitMQService
    {
        public void publishMessage(string message);
        public Task InitializeAsync();




    }
}
