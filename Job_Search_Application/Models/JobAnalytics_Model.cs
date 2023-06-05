using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Job_Search_Application.Data;
namespace Job_Search_Application.Models
{
    public class JobAnalytics_Model
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string JobAnalysis_Id  { get; set; }

        [ForeignKey("Jobs")]
        public string JobId { get; set; }
        public Jobs_Model Job { get; set; }


        [ForeignKey("Employer")]
        public string EmployerId { get; set; }
        public Employer_Model Employer { get; set; }

        [Display(Name = "Total Views")]
        public int Views { get; set; }

        [Display(Name = "Total Applies")]
        public int Applies { get; set; }

        [Display(Name = "Reviewed Candidates")]
        public int ReviewedCandidates { get; set; }

        [Display(Name = "Selected Candidates")]
        public int SelectedCandidates { get; set; }

        [Display(Name = "Interviewed Candidates")]
        public int InterviewedCandidates { get; set; }

        [Display(Name = "Hired Candidates")]
        public int HiredCandidates { get; set; }
    }

}

