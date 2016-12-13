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
            foreach (Job x in searchList)
            {
                x.Pay.ToString();
            }
            return View(searchList);
        }
       public ActionResult SearchFilter(String Title, String Category, String City, string Date , decimal Low = -1, decimal High=-1)
        {
            
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            List<Job> jobs = jobDB.Jobs.ToList();
            List<Job> testJobs = jobDB.Jobs.ToList();
            if (Category != "Category")
            {
                foreach (Job x in jobs)
                {
                    if (x.Category.ToLower() != Category.ToLower())
                    {
                        testJobs.Remove(x);
                    }
                }
                jobs = testJobs;
            }
            if (City != "City")
            {
                foreach (Job x in jobs)
                {
                    if (x.City.ToLower() != City.ToLower())
                    {
                        testJobs.Remove(x);
                    }
                }
                jobs = testJobs;
            }
            if (Low >= 0 && High >= 0)
            {
                    foreach (Job x in jobs)
                    {
                        if (x.Pay < Low || x.Pay > High)
                        {
                            testJobs.Remove(x);
                        }
                    }
                    jobs = testJobs;
            }
            return View("Search",jobs);
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
        public ActionResult Edit(Lazybones.Models.Job editedProfile)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            Lazybones.Models.Job existingJob = dbContext.Jobs.Find(editedProfile.ID);
            existingJob.Title = editedProfile.Title;
            existingJob.Description = editedProfile.Description;
            existingJob.Start_Time_Date = editedProfile.Start_Time_Date;
            existingJob.Expirey_Time_Date = editedProfile.Expirey_Time_Date;
            existingJob.Category = editedProfile.Category;
            existingJob.Pay = editedProfile.Pay;

            existingJob.Best_Bid = editedProfile.Best_Bid;
            existingJob.Picture_Location = editedProfile.Picture_Location;
            existingJob.Date_Completed = editedProfile.Date_Completed;
            //existingJob.Poster_Name = editedProfile.Poster_Name;
            existingJob.Getter_Name = editedProfile.Getter_Name;
            existingJob.Status = editedProfile.Status;
            existingJob.Payment_Status = editedProfile.Payment_Status;
            existingJob.Contact_By_Phone = editedProfile.Contact_By_Phone;
            existingJob.Contact_By_Email = editedProfile.Contact_By_Email;
            existingJob.Contact_By_Text = editedProfile.Contact_By_Text;
            existingJob.Address = editedProfile.Address;
            existingJob.City = editedProfile.City;
            existingJob.Zip = editedProfile.Zip;
            existingJob.State = editedProfile.State;
            existingJob.Getter = editedProfile.Getter;
            existingJob.Poster = editedProfile.Poster;
            existingJob.Bid_Amount = editedProfile.Bid_Amount;
            existingJob.Same_as_Home = editedProfile.Same_as_Home;




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