using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Data.Model
{
    public class Tag
    {
        public Int64 ID { get; set; }
        public string title { get; set; }
        public string metaTitle { get; set; }
        public string slug { get; set; }
        public string content { get; set; }
        public bool isDelete { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
