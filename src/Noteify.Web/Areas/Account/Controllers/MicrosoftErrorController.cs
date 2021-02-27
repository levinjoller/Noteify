using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Noteify.Web.Areas.Account.Controllers
{
    [Area("Account")]
    [AllowAnonymous]
    public class MicrosoftErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View("MicrosoftAccessDenied");
        }

        public IActionResult PermissionDenied()
        {
            Response.StatusCode = 401;
            return View("MicrosoftPermissionDenied");
        }
    }
}