using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MicroBlog.Core;
using MicroBlog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Repositories.Implementations
{
    public class GenericRepository<T>: IGenericRepository<T> where T : Entities.BaseEntity
    {

        protected DatabaseContext _context;
        protected readonly DbSet<T> _dbSet;


        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {

            return GetAll().Where(predicate).AsQueryable();
        }

        public virtual async Task<T> GetByIdAsync(long Id)
        {
            return await _dbSet.Where(a => a.Id == Id).FirstOrDefaultAsync();

        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
