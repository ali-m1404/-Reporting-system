using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace Repor.Web.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return RedirectToAction("login","account");
        }
    }
}
