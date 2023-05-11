using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Job_Search_Application.Constants;



namespace Job_Search_Application.ViewModels
{
    public class EmployeeProfileViewModel
    {
      //  public string EmployeeId { get; set; }

        //    public Roles Role { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string First_name { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string Last_name { get; set; }

     /*   [Display(Name = "Email Address")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Please enter password")]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters long")]
        [MaxLength(20, ErrorMessage = "The password cannot be longer than 20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Phone Number")]
        public string Phone_num { get; set; }*/



        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        public DateTime birthDate { get; set; }

        [Display(Name = "Profile Image")]
        public string ProfileImage { get; set; }

        [Display(Name = "Resume")]
        public string Resume { get; set; }


    }
}
