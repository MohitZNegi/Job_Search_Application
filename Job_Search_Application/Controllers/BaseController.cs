using Job_Search_Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.HttpLogging;
using Job_Search_Application.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Job_Search_Application.Constants;

namespace Job_Search_Application.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;



        public BaseController(UserManager<ApplicationUsers> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = new ApplicationDbContext();

            var userId = _userManager.GetUserId(HttpContext.User);




            if (HttpContext.User.Identity.IsAuthenticated)
            {

                var user = _context.Users.Where(u => u.Id == userId).Single();
                var IsEmployee = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "058b9440-cdef-4027-be21-0d1c0e773a9d").FirstOrDefault();
                var IsEmployer = _context.UserRoles.Where(u => u.UserId == userId && u.RoleId == "f1b1a323-474a-4b5a-844b-b2831d9fe48c").FirstOrDefault();


                var employeeProfile = _context.Employee.Include(e => e.User).Where(e => e.Employee_Id == userId).FirstOrDefault();
                var employerProfile = _context.Employer.Include(e => e.User).Where(e => e.Employer_Id == userId).FirstOrDefault();

                if (IsEmployee != null && employeeProfile != null)
                {
                    ViewBag.emp1Profile = employeeProfile;
                }
                else if (IsEmployer != null && employerProfile != null)
                {
                    ViewBag.emp2Profile = employerProfile;
                }
                else
                {
                    ViewBag.UserProfile = user;
                }
            }
            else
            {
                ViewBag.UserProfile = "";

            }



        }









    }



}

