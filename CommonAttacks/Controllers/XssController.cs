using Microsoft.AspNetCore.Mvc;

namespace CommonAttacks.Controllers
{
    public class XssController : Controller
    {
        public IActionResult DangerousGreeting(string name)
        {
            string rawGreeting = $"Welcome {name} to our application!";
            return Content(rawGreeting, "text/html");
        }

        public IActionResult SafeGreeting(string name)
        {
            string rawGreeting = $"Welcome {name} to our application!";
            string safeGreeting = System.Net.WebUtility.HtmlEncode(rawGreeting);
            return Content(safeGreeting, "text/html");
        }

        public IActionResult SafeView(string name)
        {
            ViewBag.Message = $"Welcome {name} to our application!";
            return View();
        }

        public IActionResult DangerousView(string name)
        {
            ViewBag.Message = $"Welcome {name} to our application!";
            return View();
        }
    }
}
