using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebBlog.Data.Model;
using WebBlog.Data.Models.Responses;

namespace WebBlog.Data.Repository.Implement
{
    public class BlogRepository : GenericRepository<Post>, IBlogRepository
    {
        private DataContext _context;
        public BlogRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<Post> GetAll()
        {
            try
            {
                return _context.Posts
                                .Include(u => u.User)
                                .Include(u => u.PostComments)
                                .ToList();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Post Get(long id)
        {
            var obj = _context.Posts
                            .Include(u => u.User)
                            .Include(u => u.PostComments)
                                .ThenInclude(c => c.User)
                            .Include(u => u.PostComments)
                                .ThenInclude(reply => reply.Parent)
                            .Where(p => p.ID == id)
                            .SingleOrDefault();
            if (obj == null) throw new Exception("Post Not Found!");
            return obj;
        }

        public IEnumerable<BlogItemResponse> GetAllItemBlog()
        {
            var items = _context.Posts.Include(u => u.User).Select(u => new BlogItemResponse
            {
                ID = u.ID,
                title = u.title,
                summary = u.summary,
                image = u.image,
                createAt = u.createAt.ToString("MMMM dd yyyy"),
                User = u.User
            });
            return items;
        }

        public void Create(Post post)
        {
            _context.Add(post);
        }
        public void Update(Post post)
        {
            _context.Update(post);
        }

        public void Delete(long id)
        {
            var obj = _context.Posts
                            .Where(p => p.ID == id)
                            .SingleOrDefault();
            if (obj == null) throw new Exception("Post Not Found!");
            _context.Remove(obj);
        }

        public IEnumerable<Post> List(Expression<Func<Post, bool>> predicate)
        {
            return _context.Posts
                                .Include(u => u.User)
                                .Include(u => u.PostComments)
                                .Where(predicate);
        }
    }
}
