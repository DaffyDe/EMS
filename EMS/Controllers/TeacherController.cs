using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TeacherController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeacherController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        // 1️⃣ Get userId from session safely
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // 2️⃣ Get teacher linked to logged-in user
        var teacher = _context.Teachers
            .Include(t => t.Classes)
            .FirstOrDefault(t => t.UserId == userId.Value);

        if (teacher == null)
        {
            return Unauthorized(); // or custom error page
        }

        // 3️⃣ Pass classes to view
        return View(teacher.Classes);
    }
}
