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
    public class EmployerProfileViewModel
    {
       // public string Employer_Id { get; set; }
      //  public Roles Role { get; set; }

     //   [Display(Name = "Email")]
      //  public string Email { get; set; }

        [Display(Name = "Company Name")]
        public string Company_Name { get; set; }

        [Display(Name = "Company CEO")]
        public string Company_CEO { get; set; }

        [Display(Name = "Company Description")]
        public string Company_Description { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Company Logo")]
        public IFormFile Company_Logo { get; set; }

        [Display(Name = "Company Banner")]
        public IFormFile Company_Banner { get; set; }

        [Display(Name = "Company link")]
        public string Company_URL { get; set; }

        [Display(Name = "Company Industry")]
        public string Company_Industry { get; set; }

    }
}
