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


            // Increment the reviewed count for the selected job
            var jobAnalytics = _context.Job_Analytics.FirstOrDefault(a => a.JobId == request.JobId);
            if (jobAnalytics == null)
            {
                jobAnalytics = new JobAnalytics_Model
                {
                    EmployerId = request.Job.PublisherId,
                    JobId = request.JobId,
                    Views = 0,
                    Applies = 0,
                    ReviewedCandidates = 1, // Increment the reviewed count by 1
                    Rejected = 1,// Increment the reject and selected count by 1
                    SelectedCandidates = 0
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.ReviewedCandidates++; // Increment the reviewed count by 1
                jobAnalytics.Rejected++; // Increment the rejected count by 1

            }
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
            request.InterviewRequest_Status = "pending";
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
                    ReviewedCandidates = 1,
                    SelectedCandidates = 1// Increment the reviewed and selected count by 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.ReviewedCandidates++;
                jobAnalytics.SelectedCandidates++; // Increment the reviewed and selected count by 1
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult InterviewedApplicant(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.InterviewRequest_Status = "interviewed";
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
                    InterviewedCandidates = 1 // Increment the interviewed and selected count by 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.InterviewedCandidates++; // Increment the interviewed and selected count by 1
            }
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IActionResult DeclineRequest(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.InterviewRequest_Status = "canceled";
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
                    Withdrawn = 0,
                    Rejected = 1,// Increment the reject and selected count by 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.Rejected++; // Increment the reject and selected count by 1
            }
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IActionResult Hire(string id)
        {
            var request = _context.Job_Request.Where(r => r.JobRequest_Id == id).FirstOrDefault();

            if (request == null)
                return NotFound();

            request.InterviewRequest_Status = "hired";
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
                    Withdrawn = 0,
                    HiredCandidates = 1// Increment the hire and selected count by 1
                };
                _context.Job_Analytics.Add(jobAnalytics);
            }
            else
            {
                jobAnalytics.HiredCandidates++; // Increment the hire and selected count by 1
            }
            _context.SaveChanges();
            return Ok();
        }

    }
}

