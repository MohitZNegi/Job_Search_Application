using Job_Search_Application.Data;
using Job_Search_Application.Constants;
using Job_Search_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Job_Search_Application.Areas.Identity.Pages.Account;
using Job_Search_Application.ViewModels;
using System.Data;
using NuGet.Protocol.Plugins;
using MessagePack.Internal;
using Job_Search_Application.Data.Migrations;
using NuGet.DependencyResolver;
using Microsoft.AspNetCore.Hosting.Server;

namespace Job_Search_Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AdminController(
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

        public IActionResult AllUsers()
        {
            List<Employer_Model> employers = _context.Employer.ToList();
            List<Employee_Model> employees = _context.Employee.ToList();

            // Perform further processing or pass the employer and employee lists to the view
            return View((employers, employees));
        }



        public IActionResult ReviewRequests()
        {
            List<EmployerReviewRequest_Model> reviewRequests = _context.EmployerReviewRequest
                .Include(r => r.Employer)
                .Where(r => !r.Employer.IsApproved)
                .ToList();

            // Pass the reviewRequests list to the view
            return View(reviewRequests);
        }


        public IActionResult ApproveReviewRequests(string[] approveRequests)
        {
            if (approveRequests != null && approveRequests.Length > 0)
            {
                foreach (var requestId in approveRequests)
                {
                    var reviewRequest = _context.EmployerReviewRequest
                        .Include(r => r.Employer)
                        .FirstOrDefault(r => r.JobReviewRequest_Id == requestId);

                    if (reviewRequest != null)
                    {
                        reviewRequest.Employer.IsApproved = true;
                    }
                }

                _context.SaveChanges();
            }

            return RedirectToAction("ReviewRequests");
        }

        public IActionResult MarkAsReviewed(string[] selectedRequests)
        {
            if (selectedRequests != null && selectedRequests.Length > 0)
            {
                foreach (var requestId in selectedRequests)
                {
                    var reviewRequest = _context.EmployerReviewRequest.FirstOrDefault(r => r.JobReviewRequest_Id == requestId);

                    if (reviewRequest != null)
                    {
                        reviewRequest.IsReviewed = true;
                    }
                }
                _context.SaveChanges();
            }

            return RedirectToAction("ReviewRequests");
        }



    }
}
