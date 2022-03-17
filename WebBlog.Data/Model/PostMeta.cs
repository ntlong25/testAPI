using System;

namespace WebBlog.Data.Model
{
    public class PostMeta
    {
        public Int64 ID { get; set; }
        public Int64 postId { get; set; }
        public string key { get; set; }
        public string content { get; set; }
        public bool isDelete { get; set; }
        public Post Post { get; set; }
    }
}
