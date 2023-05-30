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
        public ActionResult JobsFeed(string searchTerm1, string searchTerm2, string searchTerm3, string searchTerm4, string searchTerm5, string searchTerm6, int? page)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CurrentUser = userId;
            var jobs = _context.Jobs.Include(j => j.Employer).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm1))
            {
                jobs = _context.Jobs
                      .Where(j => j.Title.Contains(searchTerm1)
                              || j.Job_Type.Contains(searchTerm1)
                              || j.Job_Schedule.Contains(searchTerm1)
                              || j.Classification.Contains(searchTerm1)
                              || j.Job_Details.Contains(searchTerm1)
                              || j.Employer.Company_Name.Contains(searchTerm1))
                      .Include(j => j.Employer)
                      .ToList();
            }
            else if (!string.IsNullOrWhiteSpace(searchTerm2))
            {
                // Search by location
                jobs = jobs.Where(j => j.Job_Location.Contains(searchTerm2)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(searchTerm3))
            {
                jobs = jobs.Where(j => j.Job_Type.Contains(searchTerm3)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(searchTerm4))
            {
                jobs = jobs.Where(j => j.Job_Schedule.Contains(searchTerm4)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(searchTerm5))
            {
                jobs = jobs.Where(j => j.Classification.Contains(searchTerm5)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(searchTerm6))
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
                    !String.IsNullOrWhiteSpace(j.Salary) &&
                    int.TryParse(j.Salary, out var jobSalary) &&
                    jobSalary >= searchMinSalary &&
                    jobSalary <= searchMaxSalary
                ).ToList();
            }


            return View(jobs.ToPagedList(page ?? 1, 3));
        }


        public ActionResult JobsApplication(string id)
        {



            var job = _context.Jobs.Where(j => j.Jobs_Id  == id).FirstOrDefault();
            return View(job);

        }
    }
}
