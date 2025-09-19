using trainningApp.RabbitMqMessages;
using trainningApp.ViewModel;

namespace trainningApp.ConsumerMessages
{
    public class DepartmentAddedConsumer:RabbitMQBaseMessage1,IBaseConsumer<DepartmentAddedMessage> 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public override string type { get => this.type.GetType().Name; } 
        public Task consume(DepartmentAddedMessage departmentAddedMessage)
        {
            return Task.CompletedTask;
        }
    }
}
