using System;

namespace WebBlog.Data.Model
{
    public class PostTag
    {
        public Int64 postId { get; set; }
        public Int64 tagId { get; set; }
        public Tag Tag { get; set; }
        public Post Post { get; set; }
    }
}
