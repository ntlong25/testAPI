using WebBlog.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebBlog.Data.Repository.Implement
{
    public class PostCommentRepository : GenericRepository<PostComment>, IPostCommentRepository
    {
        private DataContext _context;
        public PostCommentRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<PostComment> GetAll()
        {
            try
            {
                return _context.PostComments
                                .Include(c => c.Post)
                                .ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<PostComment> Get(long postId) => _context.Set<PostComment>().Where(a => a.postId == postId).Include(a => a.User);
        
        public void Add(PostComment cmt)
        {
            _context.Add(cmt);
        }
        
        public void Update(PostComment cmt)
        {
            _context.Update(cmt);
        }

        public void Delete(long id)
        {
            var obj = _context.PostComments
                            .Where(p => p.ID == id)
                            .SingleOrDefault();
            if (obj == null) throw new Exception("Comment Not Found!");
            _context.Remove(obj);
        }

        public IEnumerable<PostComment> List(Expression<Func<PostComment, bool>> predicate)
        {
            return _context.PostComments.Where(predicate).ToList();
        }
    }
}
