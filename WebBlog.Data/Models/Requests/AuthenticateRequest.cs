using System.ComponentModel.DataAnnotations;

namespace WebBlog.Data.Models.Requests
{
    public class AuthenticateRequest
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
