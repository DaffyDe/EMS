using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // SHOW PAGE
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId.Value);
            if (teacher == null)
                return Unauthorized();

            // Get classes of this teacher
            var classes = await _context.Classes
                .Where(c => c.TeacherId == teacher.Id)
                .ToListAsync();
            ViewBag.Classes = classes;

            // Get previous announcements of these classes
            var announcements = await _context.Announcements
                .Where(a => classes.Select(c => c.Id).Contains(a.ClassId))
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            return View(announcements);
        }

        // CREATE ANNOUNCEMENT
        [HttpPost]
        public async Task<IActionResult> Create(int classId, Announcement model)
        {
            // Validate class exists
            var cls = await _context.Classes.FindAsync(classId);
            if (cls == null)
            {
                TempData["Error"] = "Selected class does not exist.";
                return RedirectToAction("Create");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all fields properly.";
                return RedirectToAction("Create");
            }

            model.ClassId = classId;
            model.DateTime = DateTime.Now;

            _context.Announcements.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Announcement posted successfully!";
            return RedirectToAction("Create");
        }

        // DELETE ANNOUNCEMENT
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ann = await _context.Announcements.FindAsync(id);

            if (ann != null)
            {
                _context.Announcements.Remove(ann);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Announcement deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Announcement not found!";
            }

            return RedirectToAction("Create");
        }
    }
}
