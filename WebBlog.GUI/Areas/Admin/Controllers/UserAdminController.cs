﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlog.GUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}