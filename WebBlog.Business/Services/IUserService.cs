using WebBlog.Data.Model;
using System.Collections.Generic;
using WebBlog.Data.Models.Responses;
using WebBlog.Data.Models.Requests;

namespace WebBlog.Business.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        UserResponse GetCurrentUser();
        UserResponse CreateUser(User user);
        UserResponse UpdateUser(User user);
        IEnumerable<User> GetUser();
        User GetUser(long id);
        void DeleteUser(long id);
        User Login(string email, string password);
    }
}
