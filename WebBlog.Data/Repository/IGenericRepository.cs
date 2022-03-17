using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebBlog.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public T Get(object id);
        public void Create(T obj);
        public void Update(T obj);
        public void Delete(object id);
        public IEnumerable<T> List(Expression<Func<T, bool>> predicate);
    }
}
