using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class ClassController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClassController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ================= DASHBOARD =================
    [HttpGet]
    public IActionResult Dashboard()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Account");

        var teacher = _context.Teachers
            .FirstOrDefault(t => t.UserId == userId.Value);

        if (teacher == null)
            return Unauthorized();

        var classes = _context.Classes
            .Where(c => c.TeacherId == teacher.Id)
            .ToList();

        return View(classes);
    }

    // ================= CREATE CLASS =================
    [HttpPost]
    public IActionResult Create(string grade, string place)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return Unauthorized();

        var teacher = _context.Teachers
            .FirstOrDefault(t => t.UserId == userId.Value);

        if (teacher == null)
            return Unauthorized();

        var newClass = new Class
        {
            TeacherId = teacher.Id,
            Grade = grade,
            Place = place
        };

        _context.Classes.Add(newClass);
        _context.SaveChanges();

        return RedirectToAction(nameof(Dashboard));
    }

    // ================= ADD STUDENT =================
    [HttpPost]
    public IActionResult AddStudent(int classId, string studentCode)
    {
        var student = _context.Students
            .FirstOrDefault(s => s.StudentCode == studentCode);

        if (student == null)
            return BadRequest("Invalid student code");

        bool alreadyAdded = _context.ClassStudents
            .Any(cs => cs.ClassId == classId && cs.StudentId == student.Id);

        if (!alreadyAdded)
        {
            _context.ClassStudents.Add(new ClassStudent
            {
                ClassId = classId,
                StudentId = student.Id
            });

            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Dashboard));
    }

    // ================= EDIT CLASS =================
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var cls = _context.Classes.FirstOrDefault(c => c.Id == id);
        if (cls == null)
            return NotFound();

        return View(cls);
    }

    [HttpPost]
    public IActionResult Edit(int id, string grade, string place)
    {
        var cls = _context.Classes.FirstOrDefault(c => c.Id == id);
        if (cls == null)
            return NotFound();

        cls.Grade = grade;
        cls.Place = place;

        _context.SaveChanges();
        return RedirectToAction(nameof(Dashboard));
    }

    // ================= DELETE CLASS (ONE-CLICK) =================
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var cls = _context.Classes.FirstOrDefault(c => c.Id == id);
        if (cls == null)
            return NotFound();

        _context.Classes.Remove(cls);
        _context.SaveChanges();

        return RedirectToAction(nameof(Dashboard));
    }
}
