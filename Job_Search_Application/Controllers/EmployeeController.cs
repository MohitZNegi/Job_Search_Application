
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
using MessagePack.Internal;
using Job_Search_Application.Data.Migrations;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net;
using Job_Search_Application.Interfaces;


namespace Job_Search_Application.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IPhotoService _photoService;   

        public EmployeeController(
            ApplicationDbContext context,
            UserManager<ApplicationUsers> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IPhotoService photoService
            )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _photoService = photoService;
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

            var viewModel = new Employee_Model
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
        public async Task<IActionResult> Create(EmployeeProfileViewModel userVM )
        {
           // var userId = _userManager.GetUserAsync(HttpContext.User);
            var userId = _userManager.GetUserId(HttpContext.User);
            // var IFUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employeeProfile = _context.Employee.Include(e => e.User).Where(e => e.Employee_Id == userId).FirstOrDefault();
            if (ModelState.IsValid && employeeProfile == null)
            {



                var result = await _photoService.AddPhotoAsync(userVM.ProfileImage);
                var pdfresult = await _photoService.AddPhotoAsync(userVM.Resume);

                var employee = new Employee_Model
                {
                    Employee_Id = userId,
                    First_name = userVM.First_name,
                    Last_name = userVM.Last_name,
                    Address = userVM.Address,
                    birthDate = userVM.birthDate,
                    Gender = userVM.Gender,
                    ProfileImage = result.Url.ToString(),
                    Resume = pdfresult.Url.ToString(),
                    UserId = userId
                };

                await _context.Employee.AddAsync(employee);
                await _context.SaveChangesAsync();

                ViewBag.UserProfile = null;
                ViewBag.comProfile = null;
                ViewBag.empProfile = employeeProfile;

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }

            return View(userVM);
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

               
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAsync(EmployeeProfileViewModel UserVM)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var result = await _photoService.AddPhotoAsync(UserVM.ProfileImage);
            var pdfresult = await _photoService.AddPhotoAsync(UserVM.Resume);
            var profile = _context.Employee.Single(e => e.Employee_Id == userId);

            profile.First_name = UserVM.First_name;
            profile.Last_name = UserVM.Last_name;
            profile.birthDate = UserVM.birthDate;
            profile.Gender = UserVM.Gender;
            profile.Address = UserVM.Address;
            profile.ProfileImage = result.Url.ToString();
            profile.Resume = pdfresult.Url.ToString();

          
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");


        }


    }
}
