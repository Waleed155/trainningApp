using trainningApp.ViewModel;

namespace trainningApp.ConsumerMessages
{
    public interface IBaseConsumer<T> where T : RabbitMQBaseMessage1
    {
        public Task consume(T message);
    }
}
