using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.Business.Services;
using WebBlog.Business.Helpers;

namespace WebBlog.Business.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //var value = "";
            //if (context.Request.Cookies["accessToken"] != null)
            //{
            //    value = context.Request.Cookies["accessToken"];
            //}
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetUser(userId.Value);
            }

            await _next(context);
        }
    }
}
