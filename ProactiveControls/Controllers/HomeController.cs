using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProactiveControls.Models;
using System.Diagnostics;

namespace ProactiveControls.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (error is ExceptionHandlerFeature ex)
            {
                if (ex.Error is SqlException sqlEx)
                {
                    _logger.LogWarning($"***** SQLException for IP: {Request.HttpContext.Connection.RemoteIpAddress}");

                    _logger.LogError($"***** Exception Type: {ex.Error.GetType()}; Message: {ex.Error.Message}, Path: {ex.Path}");
                }
            }  
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}