using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Job_Search_Application.Data
{
    public class ApplicationUsers: IdentityUser
    {


        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set;}

    }
}
