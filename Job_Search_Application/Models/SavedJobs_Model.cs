using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Job_Search_Application.Data;
namespace Job_Search_Application.Models
{
    public class SavedJobs_Model
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SavedJobs_Id  { get; set; }

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }

        public Employee_Model Employee { get; set; }

        [ForeignKey("Employer")]
        public string EmployerId { get; set; }

        public Employer_Model Employer { get; set; }

        [ForeignKey("Jobs")]
        public string JobId { get; set; }

        public Jobs_Model Job { get; set; }


    }
}
