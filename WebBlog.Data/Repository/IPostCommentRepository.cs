using WebBlog.Data.Model;
using System.Linq;

namespace WebBlog.Data.Repository
{
    public interface IPostCommentRepository : IGenericRepository<PostComment>
    {
        IQueryable<PostComment> Get(long postId);
    }
}
