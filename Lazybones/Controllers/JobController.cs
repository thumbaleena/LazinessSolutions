using Lazybones.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
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

        public ActionResult Browse()
        {
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            ViewBag.Message = "Search Postings";
            var searchList = jobDB.Jobs.ToList();
            List<Job> jobs = new List<Job>();
            LazinessSolutionsEntities4 userProf = new LazinessSolutionsEntities4();
            foreach (var job in searchList)
            {
                        jobs.Add(job);
            }
            return View(jobs);
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
        public ActionResult Edit(Lazybones.Models.Job editedJob)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            Lazybones.Models.Job existingJob = dbContext.Jobs.Find(editedJob.ID);
            existingJob.Title = editedJob.Title;
            existingJob.Description = editedJob.Description;
            existingJob.Start_Time_Date = editedJob.Start_Time_Date;
            existingJob.Expirey_Time_Date = editedJob.Expirey_Time_Date;
            existingJob.Category = editedJob.Category;
            existingJob.Pay = editedJob.Pay;

            existingJob.Best_Bid = editedJob.Best_Bid;
            existingJob.Picture_Location = editedJob.Picture_Location;
            existingJob.Date_Completed = editedJob.Date_Completed;
            //existingJob.Poster_Name = editedJob.Poster_Name;
            existingJob.Getter_Name = editedJob.Getter_Name;
            existingJob.Status = editedJob.Status;
            existingJob.Payment_Status = editedJob.Payment_Status;
            existingJob.Contact_By_Phone = editedJob.Contact_By_Phone;
            existingJob.Contact_By_Email = editedJob.Contact_By_Email;
            existingJob.Contact_By_Text = editedJob.Contact_By_Text;
            existingJob.Address = editedJob.Address;
            existingJob.City = editedJob.City;
            existingJob.Zip = editedJob.Zip;
            existingJob.State = editedJob.State;
            existingJob.Getter = editedJob.Getter;
            existingJob.Poster = editedJob.Poster;
            existingJob.Bid_Amount = editedJob.Bid_Amount;
            existingJob.Same_as_Home = editedJob.Same_as_Home;




            try
            {
                dbContext.SaveChanges();

                return RedirectToAction("Dashboard", "Home");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");

            }
            return RedirectToAction("Dashboard", "Home");
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