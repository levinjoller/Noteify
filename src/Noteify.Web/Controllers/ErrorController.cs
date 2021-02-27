using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noteify.Web.Services;
using Noteify.Web.ViewModels;

namespace Noteify.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("{controller}/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int statusCode)
        {
            var customErrorMessage = Convert.ToString(TempData["ErrorMessage"]);
            var errorMessage = ErrorService.GetErrorMessage(statusCode, customErrorMessage);

            Response.StatusCode = statusCode;

            var errorViewModel = new ErrorViewModel()
            {
                ErrorMessage = errorMessage,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            };

            return View("Error", errorViewModel);
        }
    }
}