using Job_Search_Application.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Job_Search_Application.ViewModels
{
    public class JobViewModel
    {
      


        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [StringLength(1500)]
        [Display(Name = "Job Details")]
        public string Job_Details { get; set; }

        [Display(Name = "Job Location")]
        public string Job_Location { get; set; }

        [Display(Name = "Salary")]
        public string Salary { get; set; }

        [Display(Name = "Job Type")]
        public string? Job_Type { get; set; }

        [Display(Name = "Schedule")]
        public string? Job_Schedule { get; set; }

        [Display(Name = "Job Classification")]
        public string? Classification { get; set; }
    }
}
