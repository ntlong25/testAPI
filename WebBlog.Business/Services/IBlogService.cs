using WebBlog.Data.Model;
using System.Collections.Generic;
using System.Linq;
using WebBlog.Data.Models.Responses;
using WebBlog.Data.Models.Requests;

namespace WebBlog.Business.Services
{
    public interface IBlogService
    {
        IEnumerable<Post> GetPost();
        Post GetPost(long id);
        void CreatePost(Post post);
        void UpdatePost(Post post);
        void DeletePost(long id);
        IEnumerable<BlogItemResponse> GetNewPost();
        IEnumerable<Post> SearchPost(string name);
        IEnumerable<PostComment> GetComment();
        IQueryable<PostComment> GetComment(long id);
        void AddComment(AddCommentRequest addComment);
        void AddComment(AddSubCommentRequest addSubComment);
        IEnumerable<BlogItemResponse> GetAllItemBlog();


    }
}
