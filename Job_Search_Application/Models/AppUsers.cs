using Microsoft.AspNetCore.Identity;

namespace Job_Search_Application.Models
{
    public class AppUsers : IdentityUser
    {
       public string First_Name { get; set; }
        public string Last_Name { get; set; }
    }
}