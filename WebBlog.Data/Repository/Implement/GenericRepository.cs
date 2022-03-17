using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebBlog.Data.Repository.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DataContext _context;
        private DbSet<T> dbSet;

        public GenericRepository(DataContext dataContext)
        {
            _context = dataContext;
            dbSet = dataContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
        public T Get(object id)
        {
            return dbSet.Find(id);
        }

        public void Create(T obj)
        {
            dbSet.Add(obj);
        }

        public void Delete(object id)
        {
            T obj = dbSet.Find(id);
            dbSet.Remove(obj);
        }

        public void Update(T obj)
        {
            dbSet.Update(obj);
        }
        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }
    }
}
