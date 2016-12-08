using Lazybones.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lazybones.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Create2()
        {
            string test = User.Identity.Name;
            ViewBag.Message = test;
            return View();

        }
        [HttpPost]
        public ActionResult Create(Lazybones.Models.Job job)
        {
            
            // Creates a database reference to the user db
            LazinessSolutionsEntities5 jobDB = new LazinessSolutionsEntities5();
            if (User.Identity.IsAuthenticated) {
                job.Poster_Name = User.Identity.GetUserId();
            }
            else
            {
                job.Poster_Name = "Invalid User";
            }
            
            job.Getter_Name = null;
            // Add the job passed to create post
            jobDB.Jobs.Add(job);
            // Save changes
            if (!ModelState.IsValid)
            {
                return View(job);
            }
            jobDB.SaveChanges();

            return RedirectToAction("Index", "Job");
        }

        public ActionResult Search()
        {
            ViewBag.Message = "Search Postings";

            return View("Search");
        }
    }
}