using WebBlog.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBlog.Business.Services;
using WebBlog.Data.Models.Requests;

namespace WebBlog.Data.Controllers
{
    [ApiController]
    [Route("/blog")]
    public class BlogController : Controller
    {
        private IBlogService _blogService;
        private IUserService _userService;
        public BlogController(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }
        //[Authorize]
        [HttpGet("get-all-blog")]
        public async Task<IActionResult> Index(string name)
        {
            IEnumerable<Post> blogSearch = null;
            if (!string.IsNullOrEmpty(name))
            {
                blogSearch = _blogService.SearchPost(name);
            }
            else
            {
                blogSearch = _blogService.GetPost();
            }
            return Ok(blogSearch);
        }

        //[Authorize]
        [HttpGet("get-blog-item")]
        public async Task<IActionResult> GetBlogItem()
        {
            var blogItem = _blogService.GetAllItemBlog();
            return Ok(blogItem);
        }

        //[Authorize]
        [HttpGet("detail/{id}")]
        public IActionResult Details(int id)
        {
            var blog = _blogService.GetPost(id);
            if (blog == null) return NotFound();
            //PostViewModels blogcomment = new PostViewModels(blog);
            //return Ok(blogcomment);
            return Ok(blog);
        }

        //[Authorize]
        [HttpDelete("delete-blog")]
        public IActionResult Delete(Post post)
        {
            _blogService.DeletePost((int)post.ID);
            return RedirectToAction("Index");
        }

        //[Authorize]
        [HttpPut("edit-blog")]
        public IActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                _blogService.UpdatePost(post);
                return RedirectToAction("Index");
            }
            return Ok(post);
        }

        //[Authorize]
        [HttpPost("create-blog")]
        public IActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                _blogService.CreatePost(post);
                return Ok(post);
            }
            return NotFound();
        }

        //[Authorize]
        [HttpPost("comment")]
        public IActionResult Comment(AddCommentRequest addComment)
        {
            _blogService.AddComment(addComment);
            return Ok(addComment);
        }
        //[Authorize]
        [HttpPost("subcomment")]
        public IActionResult SubComment(AddSubCommentRequest addSubComment)
        {
            _blogService.AddComment(addSubComment);
            return Ok(addSubComment);
        }
    }
}
