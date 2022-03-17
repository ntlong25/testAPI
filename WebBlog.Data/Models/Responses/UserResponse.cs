using System;
using WebBlog.Data.Model;

namespace WebBlog.Data.Models.Responses
{
    public class UserResponse
    {
        public Int64 ID { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string passwordHash { get; set; }
        public string image { get; set; }
        public DateTime registeredAt { get; set; }
        public DateTime lastLogin { get; set; }
        public string intro { get; set; }
        public string profile { get; set; }
        public bool isDelete { get; set; }
        public int roleId { get; set; }

        public UserResponse(User _user)
        {
            this.ID = _user.ID;
            this.firstName = _user.firstName;
            this.middleName = _user.middleName;
            this.lastName = _user.lastName;
            this.email = _user.email;
            this.phoneNumber = _user.phoneNumber;
            this.passwordHash = _user.passwordHash;
            this.image = _user.image;
            this.registeredAt = _user.registeredAt;
            this.lastLogin = _user.lastLogin;
            this.intro = _user.intro;
            this.profile = _user.profile;
            this.isDelete = _user.isDelete;
            this.roleId = _user.roleId;
        }
    }
}
