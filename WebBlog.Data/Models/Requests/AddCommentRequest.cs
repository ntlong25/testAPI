using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Data.Models.Requests
{
    public class AddCommentRequest
    {
        public long postId { get; set; }
        public string content { get; set; }
        public long userId { get; set; }
    }
}
