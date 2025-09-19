using Microsoft.EntityFrameworkCore;
namespace trainningApp.Models
{
    public class TrainingContext:DbContext
    {
        public TrainingContext(DbContextOptions<TrainingContext>options):base(options) 
        {

        }
        DbSet<Department> Departments { get; set; }
        DbSet<Employee> Employees { get; set; }

    }
}
