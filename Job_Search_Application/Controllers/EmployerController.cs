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

namespace Job_Search_Application.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IPhotoService _photoService;

        public EmployerController(
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

        [Authorize(Roles = "Employer")]
        public IActionResult Index()
        {


            return View();

        }

        public ActionResult ViewProfile(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var CheckIfEmployerHasProfile = _context.Employee.Where(e => e.Employee_Id == userId).FirstOrDefault();
            var profile = _context.Employer.Where(e => e.Employer_Id == userId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }

            if (CheckIfEmployerHasProfile != null)
            {

                var viewModel = new EmployeeProfileViewModel();


                return View(viewModel);
            }

            return View();
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

                ViewBag.UserProfile = null;
                ViewBag.comProfile = null;
                ViewBag.empProfile = employerProfile;

                return RedirectToAction("Index", "Home");

            }

            return View(user);
        }

        public ActionResult Update(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var CheckIfEmployerHasProfile = _context.Employer.Where(e => e.Employer_Id == userId).FirstOrDefault();


            var profile = _context.Employer.Where(e => e.Employer_Id == userId).FirstOrDefault();

            if (profile == null)
            {
                return RedirectToAction("Create", "Employer");
            }

            if (CheckIfEmployerHasProfile != null)
            {

                var viewModel = new EmployerProfileViewModel();


                return View(viewModel);
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(EmployerProfileViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var result = await _photoService.AddPhotoAsync(viewModel.Company_Logo);
            var bannerresult = await _photoService.AddPhotoAsync(viewModel.Company_Banner);

            var profile = _context.Employer.Single(e => e.Employer_Id == userId);

            var employer = new Employer_Model
            {
                Company_Name = viewModel.Company_Name,
                Company_CEO = viewModel.Company_CEO,
                Company_Description = viewModel.Company_Description,
                Company_Logo = result.Url.ToString(),
                Company_URL = viewModel.Company_URL,
                Company_Banner = bannerresult.Url.ToString(),
                Company_Industry = viewModel.Company_Industry,
                Location = viewModel.Location
            };
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");


        }

    }
}