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
                job.Status = "Created";
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
        
        public AspNetUser getCurrentUser()
        {
            LazinessSolutionsEntities4 userDB = new LazinessSolutionsEntities4();
            var u = userDB.AspNetUsers.Find(User.Identity.GetUserId());
            return u;
        }
        public ActionResult Search()
        {
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            ViewBag.Message = "Search Postings";
            List<Job> jobReturn = jobDB.Jobs.ToList();
            jobReturn = checkStatus(jobReturn);
            jobReturn = checkDate(jobReturn);
            return View(jobReturn);
        }

        public List<Job> checkStatus(List<Job> jobs)
        {
            List<Job> jobReturn = new List<Job>();
            foreach (Job x in jobs)
            {
                if (x.Status == "Created")
                {
                    jobReturn.Add(x);
                }
            }

            return jobReturn;
        }
        public List<Job> checkDate(List<Job> jobs)
        {
            List<Job> jobReturn = new List<Job>();
            foreach (Job x in jobs)
            {
                if (x.Expirey_Time_Date.Value.CompareTo(DateTime.Now)>0)
                {
                    jobReturn.Add(x);
                }
            }
            return jobReturn;
        }
        public ActionResult SearchFilter(String Title, String Category, String City, String Date, decimal Low = -1, decimal High = -1)
        {
            List<String> catList = new List<string>();
            LazinessSolutionsEntities6 jobDB = new LazinessSolutionsEntities6();
            List<Job> jobs = jobDB.Jobs.ToList();
            catList.Add("Sloth Giving"); catList.Add("Dry Cleaning"); catList.Add("Delivery");catList.Add("Test");
            /*foreach (Job x in jobs)
            {
                String cat = x.Category;
                if (!catList.Contains(cat))
                {
                    catList.Add(cat.ToLower());
                }
            }*/
            jobs = checkStatus(jobs);
            jobs = checkDate(jobs);
            if (Title != "")
            {
                jobs = searchTitle(Title, jobs);
            }
            if (Category != "Category")
            {
                jobs = searchCategory(Category, jobs, catList);
            }
            if (City != "")
            {
                jobs = searchCity(City, jobs);
            }
            if (Date != "")
            {
                jobs = searchDate(Date, jobs);
            }
            if (Low >= 0 && High >= 0)
            {
                jobs = searchPay(Low, High, jobs);
            }
            return View("Search",jobs);
        }
        public List<Job> searchTitle(String key, List<Job> jobs)
        {
            List<Job> jobReturn = new List<Job>();
            foreach (Job x in jobs)
            {
                string t = x.Title;
                string[] title = t.Split(' ');

                foreach (string word in title)
                {
                    if (word.ToLower() == key.ToLower())
                    {
                        jobReturn.Add(x);
                    }
                }
            }
            return jobReturn;
        }

        public List<Job> searchCategory(String cat, List<Job> jobs, List<String> catList)
        {

            List<Job> jobReturn = new List<Job>();
            if (cat == "Other")
            {
                 foreach (Job x in jobs)
                    {
                        /*if (!catList.Contains(x.Category))
                        {
                        jobReturn.Add(x);
                        }*/
                        if (x.Category.ToLower() != "test" && x.Category.ToLower() != "sloth giving" && x.Category.ToLower() != "dry cleaning" && x.Category.ToLower() != "delivery")
                            {
                                jobReturn.Add(x);
                            }
                    }   
            }
            else
            {
                foreach (Job x in jobs)
                {

                    if (x.Category.ToLower() == cat.ToLower())
                    {
                        jobReturn.Add(x);
                    }
                }
            }
            return jobReturn;
        }
        public List<Job> searchCity(String city, List<Job> jobs)
        {
            List<Job> jobReturn = new List<Job>();
            foreach (Job x in jobs)
            {
                if (x.City.ToLower() == city.ToLower())
                {
                    jobReturn.Add(x);
                }
            }
            return jobReturn;
        }
        public List<Job> searchDate(String Date, List<Job> jobs)
        {
            List<Job> jobReturn = new List<Job>();
            DateTime date = new DateTime();
            date = DateTime.Parse(Date);
            foreach (Job x in jobs)
            {
                if (date.Date == x.Expirey_Time_Date.Value.Date)
                {
                    jobReturn.Add(x);
                }
            }
            return jobReturn;
        }
        public List<Job> searchPay(decimal low, decimal high, List<Job> jobs)
        {
            List<Job> jobReturn = new List<Job>();
            foreach (Job x in jobs)
            {
                if (x.Pay >= low && x.Pay <= high)
                {
                    jobReturn.Add(x);
                }
            }
            return jobReturn;
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
                    if (job.Poster == u.UserName && (job.Status=="Assigned" || job.Status=="Created"))
                    {
                        jobs.Add(job);
                    }
                }
            }
            ViewBag.Jobs = jobs;
           // return View("../Home/Dashboard");
            return View();

        }

        public ActionResult Details(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            var model = dbContext.Jobs.Find(id);
            if (model.Status != "Created")
            {
                if (model.Status == "Cancelled")
                {
                    if (model.Poster != User.Identity.GetUserName() && !getCurrentUser().Admin)
                    {
                        return Redirect("../../Home/Index");
                    }
                }
                else if (model.Poster != User.Identity.GetUserName() && model.Getter != User.Identity.GetUserName() && !getCurrentUser().Admin)
                {
                    return Redirect("../../Home/Index");
                }
            }
            ViewBag.isAdmin = getCurrentUser().Admin;
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            LazinessSolutionsEntities6 dbContext = new LazinessSolutionsEntities6();
            var model = dbContext.Jobs.Find(id);
            if (model.Poster != User.Identity.GetUserName() && !getCurrentUser().Admin)
                {
                    return Redirect("../../Home/Index");
                }
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
            existingJob.Payment_Complete = editedProfile.Payment_Complete;
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
                    if ((job.Getter == u.UserName) && (job.Status == "Assigned"))
                    {
                        jobs.Add(job);
                    }
                }
            }
            ViewBag.Jobs = jobs;
            // return View("../Home/Dashboard");
            return View();
        }
         
        public ActionResult SetGetter(int ID)
        {
            LazinessSolutionsEntities6 userProf = new LazinessSolutionsEntities6();
            var u = userProf.Jobs.Find( ID);

            u.Getter = User.Identity.GetUserName();
            u.Status = "Assigned";
            userProf.SaveChanges();

            return View("Details", u);


        }
        public ActionResult Delete(int ID)
        {
            LazinessSolutionsEntities6 userProf = new LazinessSolutionsEntities6();
            var u = userProf.Jobs.Find( ID);

            u.Status = "Cancelled";
            userProf.SaveChanges();

            return View("Details", u);

        }

        public ActionResult MarkComplete(int ID)
        {
            LazinessSolutionsEntities6 userJob = new LazinessSolutionsEntities6();
            var u = userJob.Jobs.Find( ID);
            u.Status = "Complete";    
            userJob.SaveChanges();

            LazinessSolutionsEntities4 userDeet = new LazinessSolutionsEntities4();
            var z = userDeet.AspNetUsers.Find(User.Identity.GetUserId());
            var count = z.Badge_Count+1;
            z.Badge_Count = count;
            userDeet.SaveChanges();

            return View("Details", u);

        }
    }

}