using Job_Search_Application.Data;
using Job_Search_Application.Models;
using Job_Search_Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace Job_Search_Application.Controllers.API
{
    [Authorize(Roles = "Employer")]
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class EmployerController : ControllerBase
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
        [HttpPut]
        public IActionResult RejectRequest(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.Request_Status = "rejected";
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult AcceptRequest(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.Request_Status = "accepted";
            _context.SaveChanges();

            return Ok();
        }

    }
}

