using Lazybones.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lazybones.Controllers
{
    public class JobController : Controller
    {
        // GET: Job

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Lazybones.Models.Job job)
        {        
            // Creates a database reference to the user db
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            if (User.Identity.IsAuthenticated) {
                job.Poster_Name = User.Identity.GetUserId();
                job.Poster = User.Identity.GetUserName();
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
            List<Job> searchList = jobDB.Jobs.ToList();
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
            return View(searchList);
        }

        public ActionResult InnerSearch()
        {
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            ViewBag.Message = "Active Postings";
            var searchList = jobDB.Jobs.ToList();
            List<Job> jobs = new List<Job>();
            LazinessSolutionsEntities4 userProf = new LazinessSolutionsEntities4();
            var u = userProf.AspNetUsers.Find(User.Identity.GetUserId());
            foreach (var job in searchList)
            {
                if (job.Poster != null)
                {
                    if (job.Poster == u.UserName && (job.Status!="Complete" || job.Status!="Closed"))
                    {
                        jobs.Add(job);
                    }
                }
            }
            ViewBag.Jobs = jobs;
           // return View("../Home/Dashboard");
            return View();

        }

        public async Task<ActionResult> Details(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            var model = dbContext.Jobs.Find(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            var model = dbContext.Jobs.Find(id);
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditGig(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            var model = dbContext.Jobs.Find(id);
            if (TryUpdateModel(dbContext, "",
      new string[] { "Title", "Description", "Start_Time_Date", "Expirey_Time_Date", "Category", "Pay", "Best_Bid", "Date_Completed", "Status", "Payment_Status", "Contact_By_Phone", "Contact_By_Text", "Contact_By_Email", "Address", "City", "State", "Zip", "Same_As_Home", "Getter", "Poster", "Bid_Amount" }))
            {
                try
                {
                    dbContext.SaveChanges();

                    return View(model);
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(model);
        }

        public ActionResult GetterDash()
        {
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            ViewBag.Message = "My Active Postings";
            var searchList = jobDB.Jobs.ToList();
            List<Job> jobs = new List<Job>();
            LazinessSolutionsEntities4 userProf = new LazinessSolutionsEntities4();
            var u = userProf.AspNetUsers.Find(User.Identity.GetUserId());
            foreach (var job in searchList)
            {
                if (job.Getter != null)
                {
                    if (job.Getter == u.UserName && (job.Status != "Complete" || job.Status != "Closed"))
                    {
                        jobs.Add(job);
                    }
                }
            }
            ViewBag.Jobs = jobs;
            // return View("../Home/Dashboard");
            return View();
        }

 //       public ActionResult SetGigAddress(Job job)
  //      {
 //           LazinessSolutionsEntities4 userProf = new LazinessSolutionsEntities4();
//            var u = userProf.AspNetUsers.Find(User.Identity.GetUserId());
 //       }
    }

}