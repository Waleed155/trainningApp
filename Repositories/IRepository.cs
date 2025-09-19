using Microsoft.EntityFrameworkCore;
using trainningApp.Models;

namespace trainningApp.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        public IQueryable<T> GetAll();

        public T GetById(int id);

        public bool Add(T entity);

        public  Task<bool> Update(T entity);
     
        public  Task<bool> Delete(int id);
        public  Task SaveChanges();
       
    }
}
