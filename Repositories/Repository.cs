using Microsoft.EntityFrameworkCore;
using trainningApp.Models;

namespace trainningApp.Repositories
{
    public class Repository<T>:IRepository<T> where T : BaseModel
    {
        DbContext _context;
        public Repository(DbContext dbContext) {
        _context = dbContext;
        }
        public IQueryable<T> GetAll() {
            return _context.Set<T>()
                .Where(entity => !entity.IsDeleted).AsNoTracking();
        }
        public T GetById(int id) {

            return _context.
                Set<T>().AsNoTracking().
                FirstOrDefault(entity => entity.Id == id && !entity.IsDeleted)!; 
        }
        public bool Add(T entity) {
            try
            {
                _context.Set<T>().Add(entity);
                return true;
            }
            catch  { 
            return false;
            }
        }
        public async Task<bool> Update(T entity)
        {
            try
            {
                if (IsEntityTracked(entity.Id))
                {
                    var item = await _context.FindAsync<T>(entity.Id);
                    _context.Entry<T>(item).CurrentValues.SetValues(entity);
                    return true;
                }
                else
                {
                    _context.Update(entity);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task< bool> Delete(int id) {
            try
            {
                if (IsEntityTracked(id))
                {
                    var entity = await _context.
                         FindAsync<T>(id);

                    entity.IsDeleted = true;

                    return true;


                }
                else
                {
                    var entity = _context.
                        Set<T>().
                        FirstOrDefault(entity => entity.Id == id);
                    entity.IsDeleted = true;
                    return true;

                }
            }
            catch
            {
                return false;
            }
        }
        public bool IsEntityTracked(int EntityId)
        {
            return _context.ChangeTracker.Entries<T>()
                               .Any(e => e.Entity.Id == EntityId);
        }
        public async Task SaveChanges()
        {
            await  _context.SaveChangesAsync();
        }
    }
}
