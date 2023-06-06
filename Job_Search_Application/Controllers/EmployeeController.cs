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

    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public EmployeeController(
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

        [Authorize(Roles = "Employee")]
        public IActionResult ViewProfile()
        {
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employee.Where(e => e.Employee_Id == userId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create", "Employee");
            }

            var viewModel = new EmployeeProfileViewModel
            {

                First_name = profile.First_name,
                Last_name = profile.Last_name,
                Address = profile.Address,
                birthDate = profile.birthDate,
                Gender = profile.Gender,
                ProfileImage = profile.ProfileImage,
                Resume = profile.Resume

            };

            return View(viewModel);



           
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {


           // var IFUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return NotFound();
            }

            var CheckIfUserIsEmployee = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "058b9440-cdef-4027-be21-0d1c0e773a9d").FirstOrDefault();
            var CheckIfEmployeeHasProfile = _context.Employee.Where(e => e.Employee_Id == userId).FirstOrDefault();


            if (CheckIfEmployeeHasProfile == null && CheckIfUserIsEmployee != null)
            {

                var viewModel = new EmployeeProfileViewModel();


                return View(viewModel);
            }

            return Content("you have already create a profile! ");

        }

        [HttpPost]
        public  IActionResult Create(EmployeeProfileViewModel user )
        {
           // var userId = _userManager.GetUserAsync(HttpContext.User);
            var userId = _userManager.GetUserId(HttpContext.User);
            // var IFUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employeeProfile = _context.Employee.Include(e => e.User).Where(e => e.Employee_Id == userId).FirstOrDefault();
            if (ModelState.IsValid && employeeProfile == null)
            {


                var employee = new Employee_Model
                {
                    Employee_Id = userId,
                    First_name = user.First_name,
                    Last_name = user.Last_name,
                    Address = user.Address,
                    birthDate = user.birthDate,
                    Gender = user.Gender,
                    ProfileImage = user.ProfileImage,
                    Resume = user.Resume,
                    UserId = userId
                };

                _context.Employee.Add(employee);
                _context.SaveChanges();

                ViewBag.UserProfile = null;
                ViewBag.comProfile = null;
                ViewBag.employeeProfile = employeeProfile;

                return RedirectToAction("Index", "Home");

            }

            return View(user);
        }

      
        public ActionResult Update(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employee.Where(e => e.Employee_Id == userId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create", "Employee");
            }

            var viewModel = new EmployeeProfileViewModel
            {
               
                First_name = profile.First_name,
                Last_name = profile.Last_name,
                Address = profile.Address,
                birthDate = profile.birthDate,
                Gender = profile.Gender,
                ProfileImage = profile.ProfileImage,
                Resume = profile.Resume
               
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EmployeeProfileViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employee.Single(e => e.Employee_Id == userId);


            profile.First_name = viewModel.First_name;
            profile.Last_name = viewModel.Last_name;
            profile.birthDate = viewModel.birthDate;
            profile.Gender = viewModel.Gender;
            profile.ProfileImage = viewModel.ProfileImage;
            profile.Resume = viewModel.Resume;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        public ActionResult GetRequestsStatus()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var applyRequests = _context.Job_Request
                                        .Include(r => r.Job)
                                        .Include(r => r.Employer)
                                        .Where(r => r.EmployeeId == userId)
                                        .ToList();

            return View(applyRequests);
        }

        [AllowAnonymous]
        public ActionResult GetJobsApplied(string requestID)
        {
            
            var jobRequest = _context.Job_Request
                .Where(e => e.JobRequest_Id == requestID)
                .Include(e => e.Job)
                .FirstOrDefault();

            if (jobRequest == null)
                return Content("Something went wrong!");

            return View(jobRequest);
        }
    }
}
