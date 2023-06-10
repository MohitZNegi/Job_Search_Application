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


        [Display(Name = "Publish Date")]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }


        [Display(Name = "Deactivated")]
        public bool IsActive { get; set; }

        [Display(Name = "Deactivation Date")]
        public DateTime? DeactivationDate { get; set; }


     /*   [Display(Name = "Reviewed Candidates")]
        public int ReviewedCandidates { get; set; }

        [Display(Name = "Selected Candidates")]
        public int SelectedCandidates { get; set; }

        [Display(Name = "Interviewed Candidates")]
        public int InterviewedCandidates { get; set; }

        [Display(Name = "Hired Candidates")]
        public int HiredCandidates { get; set; }*/
    }

}

