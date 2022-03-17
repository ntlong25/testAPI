using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using WebBlog.Data.Model;

namespace WebBlog.Data.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public User Get(long id);
        public void Delete(long id);
        User Login(string email, string passwordHash);
        bool CheckPassword(User user, string passwordHash);
    }
}
