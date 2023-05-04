using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace Job_Search_Application.Data
{
    public class Employee
    {
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUsers User { get; set; }

        public string Address { get; set; }


        [StringLength(100)]
        public string Gender { get; set; }

        public DateTime birthDate { get; set; }

        public string ProfileImage { get; set; }

    }
}
