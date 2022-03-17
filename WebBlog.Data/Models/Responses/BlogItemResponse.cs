using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Data.Model;

namespace WebBlog.Data.Models.Responses
{
    public class BlogItemResponse
    {
        public Int64 ID { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string image { get; set; }
        public string createAt { get; set; }
        public User User { get; set; }
    }
}
