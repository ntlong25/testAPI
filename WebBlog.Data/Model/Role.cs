using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBlog.Data.Model
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public string roleName { get; set; }
        public int roleStatus { get; set; }
        public DateTime? createAt { get; set; }
        public DateTime? updateAt { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
