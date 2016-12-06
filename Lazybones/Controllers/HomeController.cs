using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lazybones.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace Lazybones.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            

            return View();
        }

        public ActionResult About()
        {

            LazinessSolutionsEntities2 dbContext = new LazinessSolutionsEntities2();



            string userId = User.Identity.GetUserId();

            ApplicationDbContext d = new ApplicationDbContext();

            string userId2=d.Users.Find(userId).Email;


            ViewBag.Message = User.Identity.GetUserId();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}