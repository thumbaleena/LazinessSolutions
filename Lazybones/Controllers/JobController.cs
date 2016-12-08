﻿using Lazybones.Models;
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Lazybones.Models.Job job)
        {
            // Creates a database reference to the user db
            LazinessSolutionsEntities3 jobDB = new LazinessSolutionsEntities3();
            // Add the job passed to create post
            jobDB.Jobs.Add(job);
            // Save changes
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