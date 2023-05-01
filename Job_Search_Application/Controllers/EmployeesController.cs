using Microsoft.AspNetCore.Mvc;

namespace Job_Search_Application.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
