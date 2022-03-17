using System;

namespace WebBlog.Data.Model
{
    public class PostComment
    {
        public Int64 ID { get; set; }
        public Int64 postId { get; set; }
        public Int64? parentId { get; set; }
        public Int64 userId { get; set; }
        public string title { get; set; }
        public int published { get; set; }
        public DateTime createAt { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }
        public bool isDelete { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public PostComment Parent { get; set; }
    }
}
