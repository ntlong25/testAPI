using System;
using System.Collections.Generic;

namespace WebBlog.Data.Model
{
    public class Category
    {
        public Int64 ID { get; set; }
        public Int64? parentId { get; set; }
        public string title { get; set; }
        public string metaTitle { get; set; }
        public string slug { get; set; }
        public string content { get; set; }
        public int status { get; set; }
        public Category Parent { get; set; }
        public ICollection<PostCategory> PostCategories { get; set; }
    }
}
