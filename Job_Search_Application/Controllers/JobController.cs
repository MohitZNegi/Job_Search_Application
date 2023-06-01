using Job_Search_Application.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc;
using X.PagedList.Mvc.Core;

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

        public ActionResult JobsFeed(string searchTerm, int? page)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CurrentUser = userId;
            var currentDate = DateTime.Now;
            var jobs = _context.Jobs.Include(j => j.Employer).ToList();

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                jobs = _context.Jobs
                    .Where(j => (j.Title.Contains(searchTerm) || j.Job_Location.Contains(searchTerm)
                        || j.Job_Type.Contains(searchTerm) || j.Job_Schedule.Contains(searchTerm)
                        || j.Classification.Contains(searchTerm) || j.Employer.Company_Name.Contains(searchTerm))
                        && j.IsPublished == true && j.DeactivationDate > currentDate)
                    .Include(j => j.Employer)
                    .ToList();
            }
            else
            {
                // Exclude draft jobs, jobs that have passed deactivation date, and set IsActive to false
                jobs = jobs.Where(j => j.IsPublished && j.DeactivationDate > currentDate).ToList();
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

            var job = _context.Jobs.Where(j => j.Jobs_Id  == id).FirstOrDefault();
            return View(job);

        }
    }
}
