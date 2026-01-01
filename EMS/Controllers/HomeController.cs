using EMS.Data;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // LANDING PAGE
    public IActionResult Index()
    {
        return View();
    }

    // PARENT VIEW PAGE
    public IActionResult Parent()
    {
        return View();
    }

    // PARENT SEARCH ACTION
    [HttpPost]
    public IActionResult Parent(string email, string phone)
    {
        var student = _context.Students
            .FirstOrDefault(s =>
                s.User.Email == email &&
                s.PhoneNumber == phone);

        if (student == null)
        {
            ViewBag.Error = "Student not found";
            return View();
        }

        var classes = _context.ClassStudents
            .Where(cs => cs.StudentId == student.Id)
            .Select(cs => cs.Class)
            .ToList();

        return View("ParentResult", classes);
    }
}
