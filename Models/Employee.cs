using System.ComponentModel.DataAnnotations.Schema;

namespace trainningApp.Models
{
    public class Employee:BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department ? Department { get; set; }
    }
}
