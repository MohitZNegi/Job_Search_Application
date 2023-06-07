using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Job_Search_Application.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Job_Search_Application.Models
{
    public class EmployerReviewRequest_Model
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string JobReviewRequest_Id { get; set; }

        [ForeignKey("Employer")]
        public string EmployerId { get; set; }
        public Employer_Model Employer { get; set; }

        public DateTime? Request_Date { get; set; }

        public bool IsReviewed { get; set; }

    }
}
