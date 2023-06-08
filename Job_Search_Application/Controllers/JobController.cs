using Job_Search_Application.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc;
using X.PagedList.Mvc.Core;
using Job_Search_Application.Models;

namespace Job_Search_Application.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public JobController(
            ApplicationDbContext context,
            UserManager<ApplicationUsers> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender
            )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult JobsFeed(string searchTerm1, string searchTerm2, string searchTerm3, string searchTerm4, string searchTerm5, string searchTerm6, int? page)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CurrentUser = userId;
            var currentDate = DateTime.Now;
            var jobs = _context.Jobs.Include(j => j.Employer).Where(j => j.IsPublished == true && j.DeactivationDate > currentDate).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm1))
            {
                jobs = _context.Jobs
                    .Where(j => (j.Title.Contains(searchTerm1) || j.Job_Location.Contains(searchTerm1)
                        || j.Job_Type.Contains(searchTerm1) || j.Job_Schedule.Contains(searchTerm1)
                        || j.Classification.Contains(searchTerm1) || j.Employer.Company_Name.Contains(searchTerm1))
                        && j.IsPublished == true && j.DeactivationDate > currentDate && j.IsActive == true)
                    .Include(j => j.Employer)
                    .ToList();
            }
            else if (!string.IsNullOrWhiteSpace(searchTerm2))
            {
                // Search by location
                jobs = _context.Jobs.Where(j => j.Job_Location.Contains(searchTerm2) ).Include(j => j.Employer).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm3))
            {
                jobs = _context.Jobs.Where(j => j.Job_Type.Contains(searchTerm3)).Include(j => j.Employer).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm4))
            {
                jobs = _context.Jobs.Where(j => j.Job_Schedule.Contains(searchTerm4) && j.IsPublished == true && j.DeactivationDate > currentDate).Include(j => j.Employer).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm5))
            {
                jobs = _context.Jobs.Where(j => j.Classification.Contains(searchTerm5) && j.IsPublished == true && j.DeactivationDate > currentDate).Include(j => j.Employer).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm6))
            {
                // Parse the search term to numeric values
                var searchMinSalary = 0;
                var searchMaxSalary = int.MaxValue;

                if (searchTerm6 == "Below $50,000")
                {
                    searchMaxSalary = 50000;
                }
                else if (searchTerm6 == "$50,000 - $100,000")
                {
                    searchMinSalary = 50000;
                    searchMaxSalary = 100000;
                }
                else if (searchTerm6 == "$100,000 - $150,000")
                {
                    searchMinSalary = 100000;
                    searchMaxSalary = 150000;
                }
                // Add more salary ranges as needed

                // Filter jobs within the salary range
                jobs = jobs.Where(j =>
                    !string.IsNullOrWhiteSpace(j.Salary) &&
                    int.TryParse(j.Salary, out var jobSalary) &&
                    jobSalary >= searchMinSalary &&
                    jobSalary <= searchMaxSalary
                && j.IsPublished == true && j.DeactivationDate > currentDate).ToList();
            }
            else
            {
                // Exclude draft jobs, jobs that have passed deactivation date, and set IsActive to false
                jobs = jobs.Where(j => j.IsPublished && j.DeactivationDate > currentDate && j.IsActive).ToList();
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


        public ActionResult JobsApplication(string id)
        {

            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CurrentUser = userId;
            var ApplyedJobsIds = _context.Job_Request.Where(e => e.EmployeeId == userId).ToList().Select(e => e.JobId);
            ViewBag.ApplyedJobs = ApplyedJobsIds;
            // Get the job request status for the current user and the specified job 
            var jobRequestStatus = _context.Job_Request
                .Where(e => e.EmployeeId == userId && e.JobId == id)
                .ToDictionary(e => e.JobId, e => e.Request_Status);

            ViewBag.RequestStatus = jobRequestStatus;

            var job = _context.Jobs.Where(j => j.Jobs_Id == id).FirstOrDefault();

            // Increment view count for the selected job
            var jobAnalytics = _context.Job_Analytics.FirstOrDefault(a => a.JobId == job.Jobs_Id);
            if (jobAnalytics == null)
            {
                jobAnalytics = new JobAnalytics_Model
                {
                    EmployerId = job.PublisherId,
                    JobId = job.Jobs_Id,
                    Views = 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.Views++;
            }

            _context.SaveChanges();
            return View(job);
        }



        
    }
}
