using WebBlog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.Data.Models.Responses;

namespace WebBlog.Data.Repository
{
    public interface IBlogRepository : IGenericRepository<Post>
    {
        public Post Get(long id);
        public void Delete(long id);
        IEnumerable<BlogItemResponse> GetAllItemBlog();
    }
}
