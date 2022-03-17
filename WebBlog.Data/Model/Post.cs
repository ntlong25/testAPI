using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Data.Model
{
    public class Post
    {
        public Int64 ID { get; set; }
        public Int64 authorId { get; set; }
        public Int64? parentId { get; set; }
        public string title { get; set; }
        public string metaTitle { get; set; }
        public string slug { get; set; }
        public string summary { get; set; }
        public string image { get; set; }
        [NotMapped]
        public IFormFile ImageUpload { get; set; }
        public int published { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }
        public bool isDelete { get; set; }
        public User User { get; set; }
        public Post Parent { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
        public ICollection<PostMeta> PostMetas { get; set; }
        public ICollection<PostCategory> PostCategories { get; set; }
    }
}
