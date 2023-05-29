using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Job_Search_Application.Data;
namespace Job_Search_Application.Models
{
    public class Jobs_Model
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Jobs_Id  { get; set; }

        [ForeignKey("Employer")]
        public string PublisherId { get; set; }

        public Employer_Model Employer { get; set; }

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
