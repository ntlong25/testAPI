using System;

namespace WebBlog.Data.Model
{
    public class PostCategory
    {
        public Int64 postId { get; set; }
        public Int64 categoryId { get; set; }
        public Category Category { get; set; }
        public Post Post { get; set; }
    }
}
