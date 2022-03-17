using WebBlog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebBlog.Business.Services;
using WebBlog.Data.Model;

namespace WebBlog.Data.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("/home")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private IBlogService _blogService;
        public HomeController(IBlogService blogService, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _blogService = blogService;
            _logger = logger;
            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }


        //[Authorize]
        //[Authorize(RoleAuthenticate.Admin)]
        [HttpGet("get-new-post")]
        public IActionResult GetNewPost()
        {
            var result = _blogService.GetNewPost();
            return Ok(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
