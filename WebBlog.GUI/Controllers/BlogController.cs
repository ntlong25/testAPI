using Microsoft.AspNetCore.Mvc;

namespace WebBlog.GUI.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(long id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
