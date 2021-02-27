using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Noteify.Web.Helpers;
using Noteify.Web.Mappings;
using Noteify.Web.Services.Interfaces;
using Noteify.Web.ViewModels.SaveViewModels;

namespace Noteify.Web.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(NoteIndexFilter filter)
        {
            ViewData["Title"] = filter.DeletedOnly ? "Entfernte Notizen" : "Notizen";
            ViewData["Designation"] = filter.Designation;
            ViewData["Date"] = filter.Date;
            ViewData["DeletedOnly"] = filter.DeletedOnly;

            var notes = await _noteService.GetNotesFiltered(filter);

            // Mapping
            var notesVM = NoteVMMapping.GetViewModel(notes);

            return View(notesVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var isValid = Guid.TryParse(id, out Guid validGuid);
            if (!isValid) return NotFound();

            var note = await _noteService.GetNote(validGuid);
            if (note == null) return NotFound();

            // Mapping
            var notesVM = NoteVMMapping.GetViewModel(note);

            return View(notesVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NoteSVM noteSVM)
        {
            if (!ModelState.IsValid) return View(noteSVM);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Mapping
            var note = NoteSVMMapping.CreateNote(noteSVM, currentUserId);

            await _noteService.Add(note);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var isValid = Guid.TryParse(id, out Guid validGuid);
            if (!isValid) return NotFound();

            var note = await _noteService.GetNote(validGuid);
            if (note == null) return NotFound();

            // Mapping
            var notesVM = NoteVMMapping.GetViewModel(note);

            return View(notesVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, [FromForm] bool isHardDelete)
        {
            var isValid = Guid.TryParse(id, out Guid validGuid);
            if (!isValid) return NotFound();

            var note = await _noteService.GetNote(validGuid);
            if (note == null) return NotFound();

            if (isHardDelete)
                await _noteService.Remove(note);
            else
                await _noteService.SoftRemove(note);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id, string returnUrl = "")
        {
            var isValid = Guid.TryParse(id, out Guid validGuid);
            if (!isValid) return NotFound();

            var note = await _noteService.GetNote(validGuid);
            if (note == null) return NotFound();

            await _noteService.RestoreNote(note);

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index");
        }
    }
}