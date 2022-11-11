using Logging.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Logging.Controllers
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
        public IActionResult Error(int? statusCode = null)
        {
            string logMsg = "";
            string requestID = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            if (statusCode.HasValue)
            {
                string OriginalURL = "";
                var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                if (statusCodeReExecuteFeature != null)
                    OriginalURL = statusCodeReExecuteFeature.OriginalPathBase + statusCodeReExecuteFeature.OriginalPath + statusCodeReExecuteFeature.OriginalQueryString;
                StatusCodeModel viewModel = new StatusCodeModel();
                viewModel.RequestId = requestID;
                viewModel.OriginalUrl = OriginalURL;
                viewModel.ErrorStatusCode = statusCode.ToString();
                
                logMsg = $"Request ID: {requestID}; OriginalURL = {OriginalURL}; " +
                $"StatusCode = {statusCode.ToString()}";
                _logger.LogError(logMsg); 
                
                return View("StatusCode", viewModel);
            }


            // check for specific errors, take special actions if necessary & log all exceptions
            var error = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, RequestDescription = error.Path, RequestError = error.Error.GetType().ToString() });
        }
    }
}