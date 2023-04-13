using Microsoft.AspNetCore.Mvc;
using WisePro_Web.Models;
using WisePro_Web.Service;

namespace WisePro_Web.Controllers
{
    public class NotesController : Controller
    {
        private readonly INote_Services _services;

        public NotesController(INote_Services services)
        {
            _services = services;
        }

        public async Task<IActionResult> Notes()
        {
            Resultnote notes = new Resultnote();

            notes.notes = await _services.GetAllTasks();
            return View(notes);
        }
        public async Task<IActionResult> Editnote(int note_id)
        {
            Resultnote notes = new Resultnote();

            if (note_id != 0)
            {
                notes.note = await _services.GetTaskById(note_id);
                notes.notes = await _services.GetAllTasks();
            }
            return View(notes);
        }
        [HttpPost]
        public async Task<IActionResult> Createnote(Resultnote ob_note)
        {

            bool result;
            if (ob_note.note.note_id == 0)
            {
                result = await _services.create(ob_note.note);
            }
            else
            {
                result = await _services.update(ob_note.note);

            }
            if (result)
            {
                return RedirectToAction("Notes");
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNote(int note_id)
        {

            bool result = await _services.delete(note_id);


            if (result)
            {
                return RedirectToAction("Notes");
            }
            else
            {
                return NoContent();
            }
        }
    }
}
