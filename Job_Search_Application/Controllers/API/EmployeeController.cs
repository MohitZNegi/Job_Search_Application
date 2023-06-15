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

    [Authorize(Roles = "Employee")]
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class EmployeeController : ControllerBase
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


        [HttpGet]
            public IEnumerable<Jobs_Model> AllJobs()
            {
                
                return _context.Jobs.Include(j => j.Employer).ToList();
            }


            [HttpPost]

        public IActionResult ApplyForJob(string id)
            {
            var userId = _userManager.GetUserId(HttpContext.User);
           

            if (userId == null)
            {
                return NotFound();
            }
            var exists = _context.Job_Request.Any(e => e.EmployeeId == userId && e.JobId == id);

                if (!ModelState.IsValid || exists)
                {
                    return BadRequest();
                }


                var selectedJob = _context.Jobs.Where(e => e.Jobs_Id == id).FirstOrDefault();

                var request = new JobRequest_Model();
                request.EmployerId = selectedJob.PublisherId;
                request.JobId = selectedJob.Jobs_Id;
                request.EmployeeId = userId;
                request.Request_Status = "pending";
                request.Applied_Date = DateTime.Now;
                request.Request_Date = DateTime.Now;
                request.InterviewRequest_Status = "null";


            _context.Job_Request.Add(request);
                _context.SaveChanges();


            // Increment the apply count for the selected job
            var jobAnalytics = _context.Job_Analytics.FirstOrDefault(a => a.JobId == id);
            if (jobAnalytics == null)
            {
                jobAnalytics = new JobAnalytics_Model
                {
                    EmployerId = selectedJob.PublisherId,
                    JobId = id,
                    Views = 0,
                    Applies = 1 // Increment the apply count by 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.Applies++; // Increment the apply count by 1
            }
            _context.SaveChanges();

            return Ok();
            }

        [HttpPost]
        public IActionResult WithdrawApplication(string id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return NotFound();
            }

            var request = _context.Job_Request.FirstOrDefault(e => e.EmployeeId == userId && e.JobId == id && e.Request_Status == "pending");

            if (request == null)
            {
                return NotFound();
            }

            request.Request_Status = "withdrawn";
            request.Request_Date = DateTime.Now;
            _context.SaveChanges();

            // Decrement the apply count for the selected job
            var jobAnalytics = _context.Job_Analytics.FirstOrDefault(a => a.JobId == id);
            if (jobAnalytics != null)
            {
                jobAnalytics.Applies--; // Decrement the apply count by 1
                _context.SaveChanges();
            }

            return Ok();
        }


        [HttpGet]
            public IEnumerable<JobRequest_Model> GetRequestsStatus()
            {
            var userId = _userManager.GetUserId(HttpContext.User);

            var applyRequests = _context.Job_Request
                                            .Include(r => r.Job)
                                            .Include(r => r.Employer)
                                            .Where(r => r.EmployeeId == userId)
                                            .ToList();

                return applyRequests;
            }
        [HttpPut]
        public IActionResult AcceptRequest(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.InterviewRequest_Status = "accepted";
            request.Request_Date = DateTime.Now;
            _context.SaveChanges();



            return Ok();
        }
        [HttpPut]
        public IActionResult DeclineRequest(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.InterviewRequest_Status = "declined";
            request.Request_Date = DateTime.Now;
            _context.SaveChanges();
            // Increment the reviewed and selected count for the selected job
            var jobAnalytics = _context.Job_Analytics.FirstOrDefault(a => a.JobId == request.JobId);
            if (jobAnalytics == null)
            {
                jobAnalytics = new JobAnalytics_Model
                {
                    EmployerId = request.Job.PublisherId,
                    JobId = request.JobId,
                    Views = 0,
                    Applies = 0,
                    ReviewedCandidates = 0,
                    SelectedCandidates = 0,
                    InterviewedCandidates = 0,
                    Withdrawn = 1 // Increment the interviewed and selected count by 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.Withdrawn++; // Increment the interviewed and selected count by 1
            }
            _context.SaveChanges();
            return Ok();
        }


    }
    }


