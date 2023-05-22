using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Job_Search_Application.Data;
namespace Job_Search_Application.Models
{
    public class Employee_Model
    {

        [Key]
        [Required]
        public string Employee_Id { get; set; }

        public virtual ApplicationUsers UserData { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUsers User { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string First_name { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string Last_name { get; set; }



        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime birthDate { get; set; }

        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }

        [Display(Name = "Resume")]
        public string Resume { get; set; }


    }
}
