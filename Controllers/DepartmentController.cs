using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using trainningApp.Models;
using trainningApp.RabbitMqMessages;
using trainningApp.Repositories;
using trainningApp.Service;
using trainningApp.ViewModel;

namespace trainningApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        IRabbitMQService _rabbitMQService;
        IRabbitMQService1 _rabbitMQService1;
        IRepository<Department> _departmentRepository;
        public DepartmentController(IRepository<Department> departmentRepository,
            IRabbitMQService rabbitMQService,
            IRabbitMQService1 rabbitMQService1)
        {
            _departmentRepository = departmentRepository;
            _rabbitMQService = rabbitMQService;
            _rabbitMQService1 = rabbitMQService1;
        }
        [HttpPost]
        public void SendMessage(DepartmentViewModel departmentViewModel)
        {
            RabbitMQBaseMessage rabbitMQBaseMessage = new RabbitMQBaseMessage()
            {
                sender = "trainningApp",
                action = "adddepart",
                sendDate = DateTime.Now,
                messageType = messageType.Department
            };
            string message = Newtonsoft.Json.JsonConvert.SerializeObject(departmentViewModel);
            _rabbitMQService.publishMessage(message);
        }
        [HttpPost("sendmessage1")]
        public void SendMessage1(DepartmentViewModel departmentViewModel)
        {
            DepartmentAddedMessage departmentAddedMessage = new DepartmentAddedMessage()
            {
                Id = departmentViewModel.Id,
                Name = departmentViewModel.Name,
                Description = departmentViewModel.Description,
                sendDate = DateTime.Now,
                sender = "trainning app",
                action = "send message",
                type = this.GetType().Name


            };

            string message =Newtonsoft.Json.JsonConvert.SerializeObject(departmentAddedMessage);
            _rabbitMQService1.publishMessage(message);

        }


         


    }
}
