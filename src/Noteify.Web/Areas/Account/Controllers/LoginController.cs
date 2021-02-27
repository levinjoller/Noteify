using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.Models.Auth;

namespace Roomber.Area.Account.Controllers
{
    [Area("Account")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        public readonly SignInManager<User> _signInManager;
        public readonly IUnitOfWork _unitOfWork;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignInMicrosoft()
        {
            var MicrosoftProp = _signInManager
                .ConfigureExternalAuthenticationProperties("MicrosoftProv", Url.Action("SignInMicrosoftResponse"));
            return Challenge(MicrosoftProp, "MicrosoftProv");
        }

        public async Task<IActionResult> SignInMicrosoftResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "Die Authentifizierung ist fehlgeschlagen.";
                return RedirectToAction("Index", "Error");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (signInResult.Succeeded)
                return RedirectToAction("Index", "Note", new { Area = "" });

            var user = new User
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Name)?.Value ??
                    info.Principal.FindFirst(ClaimTypes.Email).Value
            };

            await _unitOfWork.BeginTransaction();

            try
            {
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    _unitOfWork.Rollback();

                    TempData["ErrorMessage"] = "Entfernen Sie bitte die Sonderzeichen in Ihrem Benutzernamen bei Microsoft.";
                    return RedirectToAction("Index", "Error");
                }

                var addToLogInResult = await _userManager.AddLoginAsync(user, info);
                if (!addToLogInResult.Succeeded)
                {
                    _unitOfWork.Rollback();

                    TempData["ErrorMessage"] = "Leider konnten wir Sie nicht anmelden. Versuchen Sie es später nochmals.";
                    return RedirectToAction("Index", "Error");
                }

                await _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                return RedirectToAction("Index", "Error");
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Note", new { Area = "" });
        }
    }
}