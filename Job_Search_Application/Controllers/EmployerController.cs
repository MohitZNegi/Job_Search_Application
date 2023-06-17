﻿using Job_Search_Application.Constants;
using Job_Search_Application.Data;
using Job_Search_Application.Models;
using Job_Search_Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Job_Search_Application.Interfaces;
using Job_Search_Application.Services;
using X.PagedList;
using Job_Search_Application.Controllers.API;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace Job_Search_Application.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IPhotoService _photoService;
        private readonly AnalyticsService _analyticsService;

        public EmployerController(
            ApplicationDbContext context,
            UserManager<ApplicationUsers> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IPhotoService photoService,
            AnalyticsService analyticsService
            )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _photoService = photoService;
            _analyticsService = analyticsService;
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Index()
        {


            return View();

        }

        public ActionResult ViewProfile(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employer.Where(e => e.Employer_Id == userId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }

            var viewModel = new Employer_Model
            {

                Company_Name = profile.Company_Name,
                Company_CEO = profile.Company_CEO,
                Company_Description = profile.Company_Description,
                Company_Industry = profile.Company_Industry,
                Company_Logo = profile.Company_Logo,
                Company_URL = profile.Company_URL,
                Company_Banner = profile.Company_Banner,
                Location = profile.Location,

            };

            return View(viewModel);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Create()
        {


            // var IFUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return NotFound();
            }

            var CheckIfUserIsEmployer = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "f1b1a323-474a-4b5a-844b-b2831d9fe48c").FirstOrDefault();
            var CheckIfEmployerHasProfile = _context.Employer.Where(e => e.Employer_Id == userId).FirstOrDefault();
            var CheckIfEmployerIsReviewed = _context.Employer.Where(e => e.Employer_Id == userId && e.IsApproved == false).FirstOrDefault();
          

            if (CheckIfEmployerHasProfile == null && CheckIfUserIsEmployer != null)
            {

                var viewModel = new EmployerProfileViewModel();


                return View(viewModel);
            }

            if (CheckIfEmployerHasProfile != null && CheckIfUserIsEmployer != null && CheckIfEmployerIsReviewed != null)
            {
                TempData["ProfileMessage"] = "Your profile has been created. It is currently being reviewed." +
                    " You will be contacted through your email, so do check your email. Here is your request ID";
                return RedirectToAction("ProfileMessage");
            }

            return Content("you have already create a profile! ");

        }

        public IActionResult ProfileMessage()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var request_ID = _context.EmployerReviewRequest.Where(e => e.Employer.Employer_Id == userId).FirstOrDefault();
            return View(request_ID);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployerProfileViewModel user)
        {
            // var userId = _userManager.GetUserAsync(HttpContext.User);
            var userId = _userManager.GetUserId(HttpContext.User);
            // var IFUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employerProfile = _context.Employer.Include(e => e.User).Where(e => e.Employer_Id == userId).FirstOrDefault();
            if (ModelState.IsValid && employerProfile == null)
            {

                var result = await _photoService.AddPhotoAsync(user.Company_Logo);
                var bannerresult = await _photoService.AddPhotoAsync(user.Company_Banner);

                var employer = new Employer_Model
                {
                    Employer_Id = userId,
                   Company_Name = user.Company_Name,
                    Company_CEO = user.Company_CEO,
                    Company_Description = user.Company_Description,
                    Company_Industry = user.Company_Industry,
                    Company_Logo = result.Url.ToString(),
                    Company_URL = user.Company_URL,
                    Company_Banner = bannerresult.Url.ToString(),
                    Location = user.Location,
                    UserId = userId
                };

                await _context.Employer.AddAsync(employer);
                await _context.SaveChangesAsync();
                // Create a review request
                var reviewRequest = new EmployerReviewRequest_Model
                {
                    EmployerId = userId,
                    Request_Date = DateTime.Now,
                    IsReviewed = false,
                };

                // Save the review request in the database
                _context.EmployerReviewRequest.Add(reviewRequest);
                _context.SaveChanges();


                return RedirectToAction("GetPublishedJobs", "Employer");

            }

            return View(user);
        }

        public ActionResult Update(string id)
        {
          
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employer.Where(e => e.Employer_Id == userId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }

            var viewModel = new Employer_Model
            {

                Company_Name = profile.Company_Name,
                Company_CEO = profile.Company_CEO,
                Company_Description = profile.Company_Description,
                Company_Industry = profile.Company_Industry,
                Company_URL = profile.Company_URL,
                Location = profile.Location,
                Company_Logo = profile.Company_Logo,
                Company_Banner = profile.Company_Banner,

            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(EmployerProfileViewModel viewModel)
        {



            var userId = _userManager.GetUserId(HttpContext.User);
            var result = await _photoService.AddPhotoAsync(viewModel.Company_Logo);
            var bannerresult = await _photoService.AddPhotoAsync(viewModel.Company_Banner);

            var profile = _context.Employer.Single(e => e.Employer_Id == userId);
            profile.Company_Name = viewModel.Company_Name;
            profile.Company_CEO = viewModel.Company_CEO;
            profile.Company_Description = viewModel.Company_Description;
            profile.Company_Logo = result.Url.ToString();
            profile.Company_URL = viewModel.Company_URL;
            profile.Company_Banner = bannerresult.Url.ToString();
            profile.Company_Industry = viewModel.Company_Industry;
            profile.Location = viewModel.Location;
          
            _context.SaveChanges();

            return RedirectToAction("GetPublishedJobs", "Employer");


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteJob(string jobId)
        {
            // Find the job by its ID
            var job = _context.Jobs.Find(jobId);

            // Check if the job exists and if the current user is the publisher of the job
            if (job == null || job.PublisherId != _userManager.GetUserId(HttpContext.User))
            {
                return NotFound();
            }

            // Remove the job from the context and save changes
            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return RedirectToAction("GetPublishedJobs", "Employer");
        }

        public ActionResult Publish_Job()
        {
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employer.Where(e => e.Employer_Id == userId && e.IsApproved == true).FirstOrDefault();
            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Publish_Job(JobViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var IsProfileCreated = _context.Employer.Any(e => e.Employer_Id == userId);

            if (!IsProfileCreated)
            {
                return RedirectToAction("Create", "Employer");
            }

            var IfUserIsEmployer = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "f1b1a323-474a-4b5a-844b-b2831d9fe48c").FirstOrDefault();
            if (!ModelState.IsValid || IfUserIsEmployer == null)
            {
                return View(viewModel);
            }

            var job = new Jobs_Model
            {
                Title = viewModel.Title,
                Job_Details = viewModel.Job_Details,
                Job_Location = viewModel.Job_Location,
                Salary = viewModel.Salary,
                Job_Type = viewModel.Job_Type,
                Job_Schedule = viewModel.Job_Schedule,
                Classification = viewModel.Classification,
                PublishDate = DateTime.Now, // Set PublishDate as null since it is not being published yet
                IsPublished = true,
                IsActive = true, // Set IsActive to false since it is not being published yet
                DeactivationDate = viewModel.DeactivationDate,
                PublisherId = userId
            };

            _context.Jobs.Add(job);
            _context.SaveChanges();

            return RedirectToAction("GetPublishedJobs", "Employer");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublishDraft_Job(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var IsProfileCreated = _context.Employer.Any(e => e.Employer_Id == userId);

            if (!IsProfileCreated)
            {
                return RedirectToAction("Create", "Employer");
            }

            var IfUserIsEmployer = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "f1b1a323-474a-4b5a-844b-b2831d9fe48c").FirstOrDefault();

            var draftjob = _context.Jobs.Find(id);

            draftjob.IsPublished = true;
            draftjob.PublishDate = DateTime.Now;
            draftjob.IsActive = true;
           
            _context.SaveChanges();

            return RedirectToAction("GetPublishedJobs", "Employer");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DraftJobs(JobViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var IsProfileCreated = _context.Employer.Any(e => e.Employer_Id == userId);

            if (!IsProfileCreated)
            {
                return RedirectToAction("Create", "Employer");
            }

            var IfUserIsEmployer = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "f1b1a323-474a-4b5a-844b-b2831d9fe48c").FirstOrDefault();
            if (!ModelState.IsValid || IfUserIsEmployer == null)
            {
                return View(viewModel);
            }

            var job = new Jobs_Model
            {
                Title = viewModel.Title,
                Job_Details = viewModel.Job_Details,
                Job_Location = viewModel.Job_Location,
                Salary = viewModel.Salary,
                Job_Type = viewModel.Job_Type,
                Job_Schedule = viewModel.Job_Schedule,
                Classification = viewModel.Classification,
                PublishDate = null, // Set PublishDate as null since it is not being published yet
                IsPublished = false,
                IsActive = false, // Set IsActive to false since it is not being published yet
                DeactivationDate = viewModel.DeactivationDate,
                PublisherId = userId
            };

            _context.Jobs.Add(job);
            _context.SaveChanges();

            return RedirectToAction("GetPublishedJobs", "Employer");
        }

        public ActionResult GetDraftJobs(string searchTerm, int? page)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CurrentUser = userId;
            var jobs = _context.Jobs.Include(j => j.Employer).Where(j => j.PublisherId == userId).ToList();


            var profile = _context.Employer.Where(e => e.Employer_Id == userId && e.IsApproved == true).FirstOrDefault();
            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }

            var publishedjobs = _context.Jobs.Where(e => e.PublisherId == userId).FirstOrDefault();

          

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                jobs = _context.Jobs.Where(j => j.PublisherId == userId)
                    .Where(j => (j.Title.Contains(searchTerm) || j.Job_Location.Contains(searchTerm)
                        || j.Job_Type.Contains(searchTerm) || j.Job_Schedule.Contains(searchTerm)
                        || j.Classification.Contains(searchTerm) || j.Employer.Company_Name.Contains(searchTerm))
                        && j.IsPublished == false && j.IsActive == false)
                    .Include(j => j.Employer)
                    .ToList();
            }
            else
            {
                // Only draft jobs
                jobs = jobs.Where(j => j.IsPublished == false && j.IsActive == false).ToList();
            }
            return View(jobs.ToPagedList(page ?? 1, 3));

        }

        public ActionResult GetPublishedJobs(string searchTerm, string sortOrder, int? page)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CurrentUser = userId;
            var currentDate = DateTime.Now;
            var jobs = _context.Jobs.Include(j => j.Employer).Where(j => j.PublisherId == userId).ToList();


            var profile = _context.Employer.Where(e => e.Employer_Id == userId && e.IsApproved == true).FirstOrDefault();
            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }



            // Apply sorting based on the sortOrder parameter
            switch (sortOrder)
            {
                case "title_asc":
                    jobs = jobs.OrderBy(j => j.Title).ToList();
                    break;
                case "title_desc":
                    jobs = jobs.OrderByDescending(j => j.Title).ToList();
                    break;
                case "date_asc":
                    jobs = jobs.OrderBy(j => j.PublishDate).ToList();
                    break;
                case "closedate_asc":
                    jobs = jobs.OrderByDescending(j => j.DeactivationDate).ToList();
                    break;
                default:
                    // Default sorting by descending published date
                    jobs = jobs.OrderByDescending(j => j.PublishDate).ToList();
                    break;
            }

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                jobs = _context.Jobs.Where(j => j.PublisherId == userId)
                    .Where(j => (j.Title.Contains(searchTerm) || j.Job_Location.Contains(searchTerm)
                        || j.Job_Type.Contains(searchTerm) || j.Job_Schedule.Contains(searchTerm)
                        || j.Classification.Contains(searchTerm) || j.Employer.Company_Name.Contains(searchTerm))
                        && j.IsPublished == true)
                       
                    .Include(j => j.Employer)
                    .ToList();
            }
            else
            {
                // Exclude draft deactivated jobs
                jobs = jobs.Where(j => j.IsPublished && j.DeactivationDate > currentDate).Where(j => j.IsActive || j.IsActive == false).ToList();
                foreach (var job in jobs)
                {
                    if (job.DeactivationDate <= currentDate)
                    {
                        job.IsActive = false;
                    }
                }
                _context.SaveChanges();
            }
            var jobViews = _analyticsService.GetJobViews(jobs);
            var jobApplies = _analyticsService.GetJobApplies(jobs);
            var reviewedCandidates = _analyticsService.GetReviewedCandidates(jobs);
            var selectedCandidates = _analyticsService.GetSelectedCandidates(jobs);
            var rejected = _analyticsService.GetRejectedCandidates(jobs);
            var withdrawn = _analyticsService.GetWithdrawnApplicants(jobs);
            var interviewed = _analyticsService.GetInterviewedCandidates(jobs);
            var hired = _analyticsService.GetHiredCandidates(jobs);

            ViewBag.JobViews = jobViews;
            ViewBag.JobApplies = jobApplies;
            ViewBag.ReviewedCandidates = reviewedCandidates;
            ViewBag.SelectedCandidates = selectedCandidates;
            ViewBag.Rejected = rejected;
            ViewBag.Withdrawn = withdrawn;
            ViewBag.Interviewed = interviewed;
            ViewBag.Hired = hired;
            // Pass the sortOrder to the view
            ViewBag.CurrentSort = sortOrder;
            return View(jobs.ToPagedList(page ?? 1, 3));

        }
        public ActionResult GetDeactivatedJobs(string searchTerm, int? page)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var profile = _context.Employer.Where(e => e.Employer_Id == userId && e.IsApproved == true).FirstOrDefault();
            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }
            ViewBag.CurrentUser = userId;
            var currentDate = DateTime.Now;
            var jobs = _context.Jobs.Include(j => j.Employer).ToList();

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                jobs = _context.Jobs.Where(j => j.PublisherId == userId)
                    .Where(j => (j.Title.Contains(searchTerm) || j.Job_Location.Contains(searchTerm)
                        || j.Job_Type.Contains(searchTerm) || j.Job_Schedule.Contains(searchTerm)
                        || j.Classification.Contains(searchTerm) || j.Employer.Company_Name.Contains(searchTerm))
                        && j.IsPublished == true && j.DeactivationDate < currentDate)
                    .Include(j => j.Employer)
                    .ToList();
            }
            else
            {
                // Exclude draft jobs, include jobs that have passed deactivation date, and set IsActive to false
                jobs = jobs.Where(j => j.IsPublished && j.DeactivationDate < currentDate).ToList();
                foreach (var job in jobs)
                {
                    if (job.DeactivationDate <= currentDate)
                    {
                        job.IsActive = false;
                    }
                }
                _context.SaveChanges();
            }

            return View(jobs.ToPagedList(page ?? 1, 3));
        }

        // GET: Job/Edit/5
        public IActionResult EditJobs(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var job = _context.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            // Check if the current user is the publisher of the job
            var userId = _userManager.GetUserId(User);
            if (job.PublisherId != userId)
            {
                return Forbid();
            }

            var viewModel = new Jobs_Model
            {
                Jobs_Id = id,
                Title = job.Title,
                Job_Details = job.Job_Details,
                Job_Location = job.Job_Location,
                Salary = job.Salary,
                Job_Type = job.Job_Type,
                Job_Schedule = job.Job_Schedule,
                Classification = job.Classification,
                DeactivationDate = job.DeactivationDate,
                IsActive = job.IsActive,
                
                
            };

            return View(viewModel);
        }

        // POST: Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditJobs(JobViewModel viewModel, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Check if the current user is the publisher of the job
            var userId = _userManager.GetUserId(User);
            var job = _context.Jobs.Find(id);
            if (job.PublisherId != userId)
            {
                return Forbid();
            }

            job.Title = viewModel.Title;
            job.Job_Details = viewModel.Job_Details;
            job.Job_Location = viewModel.Job_Location;
            job.Salary = viewModel.Salary;
            job.Job_Type = viewModel.Job_Type;
            job.Job_Schedule = viewModel.Job_Schedule;
            job.Classification = viewModel.Classification;
            job.DeactivationDate = viewModel.DeactivationDate;
            job.IsPublished = true;
            job.IsActive = viewModel.IsActive;
          

            _context.SaveChanges();

       
            return RedirectToAction("GetPublishedJobs", "Employer");
        }


        // GET: Job/Edit/5
        public IActionResult EditDraftJobs(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var job = _context.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            // Check if the current user is the publisher of the job
            var userId = _userManager.GetUserId(User);
            if (job.PublisherId != userId)
            {
                return Forbid();
            }

            var viewModel = new JobViewModel
            {
               
                Title = job.Title,
                Job_Details = job.Job_Details,
                Job_Location = job.Job_Location,
                Salary = job.Salary,
                Job_Type = job.Job_Type,
                Job_Schedule = job.Job_Schedule,
                Classification = job.Classification,
                DeactivationDate = job.DeactivationDate,
                IsPublished = true,
                IsActive = true,
                PublishDate = DateTime.Now
            


            };

            return View(viewModel);
        }
        // POST: Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDraftJobs(JobViewModel viewModel, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Check if the current user is the publisher of the job
            var userId = _userManager.GetUserId(User);
            var job = _context.Jobs.Find(id);
            if (job.PublisherId != userId)
            {
                return Forbid();
            }

            job.Title = viewModel.Title;
            job.Job_Details = viewModel.Job_Details;
            job.Job_Location = viewModel.Job_Location;
            job.Salary = viewModel.Salary;
            job.Job_Type = viewModel.Job_Type;
            job.Job_Schedule = viewModel.Job_Schedule;
            job.Classification = viewModel.Classification;
            job.DeactivationDate = viewModel.DeactivationDate;
            job.IsPublished = true;
            job.IsActive = true;
            job.PublishDate = DateTime.Now;

            _context.SaveChanges();


            return RedirectToAction("GetPublishedJobs", "Employer");
        }


        public ActionResult All_Requests(string jobApplication)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var profile = _context.Employer.FirstOrDefault(e => e.Employer_Id == userId && e.IsApproved == true);
            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }

            var publishedJobs = _context.Jobs.Where(j => j.IsPublished).ToList();
            ViewBag.PublishedJobs = publishedJobs;

            var requests = _context.Job_Request
                .Include(e => e.Job)
                .Include(e => e.Employee)
                .Where(e => e.Employer.Employer_Id == userId &&
                            (e.Request_Status == "pending" || e.Request_Status == "accepted" || e.Request_Status == "rejected"));

            if (!string.IsNullOrEmpty(jobApplication))
            {
                requests = requests.Where(e => e.Job.Title == jobApplication);
            }

            var filteredRequests = requests.ToList();

            return View(filteredRequests);
        }


        public ActionResult SelectedCandidates()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var requests = _context.Job_Request
                                    .Include(e => e.Job)
                                    .Include(e => e.Employee)
                                    .Where(e => e.Employer.Employer_Id == userId && e.Request_Status == "accepted")
                                    .ToList();

            return View(requests);
        }


        [AllowAnonymous]
        public ActionResult GetEmployee_Profile(string requestID)
        {
            var jobRequest = _context.Job_Request
                .Where(e => e.JobRequest_Id == requestID)
                .Include(e => e.Employer)
                .Include(e => e.Job)
                .Include(e => e.Employee)
                .ThenInclude(e => e.User)
                .FirstOrDefault();

            if (jobRequest == null)
                return Content("Something went wrong!");

            return View(jobRequest);
        }

    }
}