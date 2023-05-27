using Job_Search_Application.Constants;
using Job_Search_Application.Data;
using Job_Search_Application.Models;
using Job_Search_Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Search_Application.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public EmployerController(
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

            var viewModel = new EmployerProfileViewModel
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


            if (CheckIfEmployerHasProfile == null && CheckIfUserIsEmployer != null)
            {

                var viewModel = new EmployerProfileViewModel();


                return View(viewModel);
            }

            return Content("you have already create a profile! ");

        }

        [HttpPost]
        public IActionResult Create(EmployerProfileViewModel user)
        {
            // var userId = _userManager.GetUserAsync(HttpContext.User);
            var userId = _userManager.GetUserId(HttpContext.User);
            // var IFUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employerProfile = _context.Employer.Include(e => e.User).Where(e => e.Employer_Id == userId).FirstOrDefault();
            if (ModelState.IsValid && employerProfile == null)
            {


                var employer = new Employer_Model
                {
                    Employer_Id = userId,
                   Company_Name = user.Company_Name,
                    Company_CEO = user.Company_CEO,
                    Company_Description = user.Company_Description,
                    Company_Industry = user.Company_Industry,
                    Company_Logo = user.Company_Logo,
                    Company_URL = user.Company_URL,
                    Company_Banner = user.Company_Banner,
                    Location = user.Location,
                    UserId = userId
                };

                _context.Employer.AddAsync(employer);
                _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");

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

            var viewModel = new EmployerProfileViewModel
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EmployerProfileViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);


            var profile = _context.Employer.Single(e => e.Employer_Id == userId);


            profile.Company_Name = viewModel.Company_Name;
            profile.Company_CEO = viewModel.Company_CEO;
            profile.Company_Description = viewModel.Company_Description;
            profile.Company_Logo = viewModel.Company_Logo;
            profile.Company_URL = viewModel.Company_URL;
            profile.Company_Banner = viewModel.Company_Banner;
            profile.Company_Industry = viewModel.Company_Industry;
            profile.Location = viewModel.Location;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");


        }

    }
}