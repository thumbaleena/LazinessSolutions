using Lazybones.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
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

            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult Search()
        {

            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            ViewBag.Message = "Search Postings";
            var searchList = jobDB.Jobs.ToList();
            List<Job> jobs = new List<Job>();

            LazinessSolutionsEntities4 userProf = new LazinessSolutionsEntities4();

            var u = userProf.AspNetUsers.Find(User.Identity.GetUserId());
            
            foreach (var job in searchList)
            {
                if (job.City != null)
                {
                    if (job.City == u.City)
                    {
                        jobs.Add(job);
                    }
                }
            }
            
            return View(jobs);
        }

        public ActionResult InnerSearch()
        {

            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            ViewBag.Message = "Search Postings";
            var searchList = jobDB.Jobs.ToList();
            List<Job> jobs = new List<Job>();

            LazinessSolutionsEntities4 userProf = new LazinessSolutionsEntities4();

            var u = userProf.AspNetUsers.Find(User.Identity.GetUserId());

            foreach (var job in searchList)
            {
                if (job.City != null)
                {
                    if (job.City == u.City)
                    {
                        jobs.Add(job);
                    }
                }
            }
         //   return View(jobs);
            return View("../Home/Dashboard", jobs);
        }
        //       public ActionResult Details()
        //       {
        //           return View("Details");
        //       }

        public async Task<ActionResult> Details(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();


            //figure out how to set keyvalue from referring link

            var model = dbContext.Jobs.Find(id);
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();

            var model = dbContext.Jobs.Find(id);

            return View(model);
        }
    }
}