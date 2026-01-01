using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Email == email);

        if (user == null || user.PasswordHash != HashPassword(password))
        {
            ViewBag.Error = "Invalid email or password";
            return View();
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Role", user.Role);

       
        if (user.Role == "Teacher")
            return RedirectToAction("Dashboard", "Teacher");
            return RedirectToAction("Dashboard", "Student");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string role, string name, string email,
                                  string phone, string password)
    {
        if (_context.Users.Any(u => u.Email == email))
        {
            ViewBag.Error = "Email already exists";
            return View();
        }

        var user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        if (role == "Teacher")
        {
            _context.Teachers.Add(new Teacher
            {
                UserId = user.Id,
                Name = name,
                PhoneNumber = phone,
                TeacherCode = "TCH-" + Guid.NewGuid().ToString("N")[..6]
            });
        }
        else
        {
            _context.Students.Add(new Student
            {
                UserId = user.Id,
                Name = name,
                PhoneNumber = phone,
                StudentCode = "STD-" + Guid.NewGuid().ToString("N")[..6]
            });
        }

        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
