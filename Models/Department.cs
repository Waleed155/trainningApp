namespace trainningApp.Models
{
    public class Department:BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Employee>? Employees { get; set; }

    }
}
