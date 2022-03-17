using System;
using System.Text.Json.Serialization;
using WebBlog.Data.Model;

namespace WebBlog.Data.Models.Responses
{
    public class AuthenticateResponse
    {

        public User UserInfor { get; set; }
        public string JwtToken { get; set; }


        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {

            UserInfor = new User
            {
                ID = user.ID,
                firstName = user.firstName,
                middleName = user.middleName,
                lastName = user.lastName,
                phoneNumber = user.phoneNumber,
                email = user.email,
                registeredAt = DateTime.Now,
                lastLogin = DateTime.Now,
                intro = user.intro,
                profile = user.profile,
                isDelete = false,
                roleId = user.roleId
            };
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
