using Job_Search_Application.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Job_Search_Application.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {

            var userManager = service.GetService<UserManager<ApplicationUsers>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Employer.ToString()));

            // creating admin

            var user = new ApplicationUsers
            {
                UserName = "admin-capstone@gmail.com",
                Email = "admin-capstone@gmail.com",
                First_name = "Admin",
                Last_name = "Capstone",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin-Capstone@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
