using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InstagramClone.Models;

namespace InstagramClone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("/Views/Home/index.html");
        }

        public IActionResult ForgotPassword()
        {
            return View("/Views/Home/forgotPassword.html");
        }

        public IActionResult LoginAndRegistration()
        {
            return View("/Views/Home/loginAndRegistration.html");
        }        

        public IActionResult PostDetails()
        {
            return View("/Views/Home/postDetails.html");
        }

        public IActionResult PostList()
        {
            return View("/Views/Home/postList.html");
        }

        public IActionResult Upload()
        {
            return View("/Views/Home/upload.html");
        }
    }
}
