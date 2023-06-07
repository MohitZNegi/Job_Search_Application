using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Job_Search_Application.Data;
namespace Job_Search_Application.Models
{
    public class Employer_Model
    {

        [Key]
        [Required]
        public string Employer_Id { get; set; }

        public virtual ApplicationUsers UserData { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUsers User { get; set; }

        [Display(Name = "Company Name")]
        public string Company_Name { get; set; }

        [Display(Name = "Company CEO")]
        public string Company_CEO { get; set; }

        [Display(Name = "Company Description")]
        public string Company_Description { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Company Logo")]
        public string Company_Logo { get; set; }

        [Display(Name = "Company Banner")]
        public string Company_Banner { get; set; }

        [Display(Name = "Company link")]
        public string Company_URL { get; set; }

        [Display(Name = "Company Industry")]
        public string Company_Industry { get; set; }

        public bool IsApproved { get; set; }

    }
}
