using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.Models.Auth;
using Noteify.Web.Areas.Account.Helpers;
using Noteify.Web.Areas.Account.Services;
using Noteify.Web.Areas.Account.Services.Interface;

namespace Noteify.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class ManageController : Controller
    {
        private readonly IManageService _manageService;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<User> _signInManager;

        public ManageController(IManageService manageService, UserManager<User> userManager,
            IUnitOfWork unitOfWork, SignInManager<User> signInManager)
        {
            _manageService = manageService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ExportCsv()
        {
            var notes = await _manageService.GetAllUndeletedNotes();
            if (!notes.Any())
            {
                ViewData["NoEntries"] = "Es sind keine Notizen vorhanden!";
                return View("Index");
            }

            // Remove error message
            ViewData["NoEntries"] = null;

            var exportConfig = new ExportConfig(
                stringBuilder: new StringBuilder(),
                separator: ",",
                columnHeader: new string[]
                {
                    "Titel",
                    "Inhalt",
                    "Erstellt am"
                },
                content: notes
            );

            ManageService.AddSeparatorAndColumnHeaderToStringBuilder(exportConfig);
            ManageService.AddQuotedNotesToStringBuilder(exportConfig);

            return File(Encoding.UTF8.GetBytes(exportConfig.StringBuilder.ToString()),
                "text/csv", $"Noteify.{DateTime.Now:dd-MM-yyyy}.csv");
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var userLogins = await _userManager.GetLoginsAsync(user);

            await _unitOfWork.BeginTransaction();

            try
            {
                foreach (var login in userLogins)
                {
                    var removeLoginResult = await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                    if (!removeLoginResult.Succeeded)
                    {
                        _unitOfWork.Rollback();
                        TempData["ErrorMessage"] = "Ihr Konto konnte nicht entfernt werden.";
                        return RedirectToAction("Index", "Error");
                    }
                }

                var deleteUserResult = await _userManager.DeleteAsync(user);
                if (!deleteUserResult.Succeeded)
                {
                    _unitOfWork.Rollback();
                    TempData["ErrorMessage"] = "Ihr Konto konnte nicht entfernt werden.";
                    return RedirectToAction("Index", "Error");
                }

                await _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                TempData["ErrorMessage"] = "Ihr Konto konnte nicht entfernt werden.";
                return RedirectToAction("Index", "Error");
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}