using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebBlog.Data.Model
{
    public class User
    {
        public Int64 ID { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string passwordHash { get;set; }
        public string image { get; set; }
        [NotMapped]
        public IFormFile ImageUpload { get; set; }
        public DateTime registeredAt { get; set; }
        public DateTime lastLogin { get; set; }
        public string intro { get; set; }
        public string profile { get; set; }
        public bool isDelete { get; set; }
        public int roleId { get; set; }
        [ForeignKey("roleId")]
        public Role Role { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
