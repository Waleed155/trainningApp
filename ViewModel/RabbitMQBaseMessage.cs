namespace trainningApp.ViewModel
{
    public class RabbitMQBaseMessage
    {
      public DateTime sendDate {  get; set; }=DateTime.Now;
        public string sender { get; set; }
        public string action { get; set; }
        public string data { get; set; }
        public messageType messageType { get; set; }
    }
    public class RabbitMQBaseMessage1
    {
        public DateTime sendDate { get; set; } = DateTime.Now;
        public string sender { get; set; }
        public string action { get; set; }
        public  virtual string type { get; set; }
    }
    public enum messageType
    {
        None = 0,
        Department= 1,
        Employee= 2,
    }
}
