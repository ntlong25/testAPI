using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebBlog.Business.Services;
using WebBlog.Data.Model;
using WebBlog.Data.Models.Requests;
using static WebBlog.Business.Helpers.SystemEnum;

namespace WebBlog.Data.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/user")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromForm] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(RevokeTokenRequest model)
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            _userService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }


        [AllowAnonymous]
        [HttpGet("get-current-user")]
        public IActionResult GetCurrentUser()
        {
            var user = _userService.GetCurrentUser();
            return Ok(user);
        }
        [Authorize]
        //[AllowAnonymous]
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var users = _userService.GetUser();
            return Ok(users);
        }
        [Authorize]
        //[AllowAnonymous]
        [HttpGet("detail/{id}")]
        public IActionResult Details(long id)
        {
            var detail = _userService.GetUser(id);
            if(detail == null) return NotFound();
            return Ok(detail);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetUser(id);
            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = _userService.GetUser(id);
            return Ok(user.RefreshTokens);
        }



        [Authorize]
        //[AllowAnonymous]
        [HttpDelete("delete-user/{id}")]
        public IActionResult Delete(long id)
        {
            var user = _userService.GetUser(id);
            if (user == null) return NotFound();
            _userService.DeleteUser(user.ID);
            return Ok();
        }

        [Authorize]
        //[AllowAnonymous]
        [HttpPut("edit-user")]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _userService.UpdateUser(user);
                return Ok(user);
            }
            return NotFound();
        }

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("create-user")]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _userService.CreateUser(user);
                return Ok(user);
            }
            return NotFound();
        }
    }
}
