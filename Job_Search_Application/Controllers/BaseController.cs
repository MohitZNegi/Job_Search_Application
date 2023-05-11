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



                public  BaseController(UserManager<ApplicationUsers> userManager,
                    RoleManager<IdentityRole> roleManager,
                    IEmailSender emailSender)
                {
                    _roleManager = roleManager;
                    _userManager = userManager;
                    _emailSender = emailSender;
                    _context = new ApplicationDbContext();

                    var userId = _userManager.GetUserId(HttpContext.User);
            var roleId = _userManager.GetUsersInRoleAsync(userId).Result;
            

                    if (HttpContext.User.Identity.IsAuthenticated)
                    {


                        var user = _context.Users.Where(u => u.Id == userId).Single();



                        var employeeProfile = _context.Employee.Include(e => e.User).Where(e => e.UserId == userId).FirstOrDefault();
                        var employerProfile = _context.Employer.Include(e => e.User).Where(e => e.UserId == userId).FirstOrDefault();

                        if ( user == roleId && employeeProfile != null)
                        {
                            ViewBag.emp1Profile = employeeProfile;
                        }
                        else if (user == roleId && employerProfile != null)
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

