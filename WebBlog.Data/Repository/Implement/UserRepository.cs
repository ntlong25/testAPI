using WebBlog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebBlog.Data.Repository.Implement
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return _context.Users.Where(u => u.isDelete == false).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public User Get(long id)
        {
            var obj = _context.Users.Where(p => p.ID == id && p.isDelete == false).SingleOrDefault();
            if (obj == null) throw new Exception("User Not Found!");
            return obj;
        }

        public void Create(User user)
        {
            _context.Add(user);
        }
        public void Update(User user)
        {
            _context.Update(user);
        }

        public void Delete(long id)
        {
            var obj = Get(id);
            if (obj == null) throw new Exception("User Not Found!");
            obj.isDelete = true;
        }

        public IEnumerable<User> List(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Where(predicate).ToList();
        }
        public bool CheckPassword(User user, string passwordHash)
        {
            if (user.passwordHash.Equals(passwordHash))
            {
                return true;
            }
            return false;
        }
        public User Login(string email, string passwordHash)
        {
            try
            {
                var existUser = List(u => u.email.Equals(email)).SingleOrDefault();
                if(existUser == null)
                {
                    throw new Exception("User not found!");
                }
                if(!CheckPassword(existUser, passwordHash))
                {
                    throw new Exception("Password incorrect!");
                }
                existUser.lastLogin = DateTime.Now;
                return existUser;

            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
