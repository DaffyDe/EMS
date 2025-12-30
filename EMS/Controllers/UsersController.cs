using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // REGISTER
    [HttpPost]
    public IActionResult Register(string email, string password, string role,
                                   string name, string phone)
    {
        if (_context.Users.Any(u => u.Email == email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Email = email,
            PasswordHash = Hash(password),
            Role = role
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        if (role == "Student")
        {
            _context.Students.Add(new Student
            {
                UserId = user.Id,
                Name = name,
                PhoneNumber = phone,
                StudentCode = "STD-" + Guid.NewGuid().ToString("N")[..6]
            });
        }
        else
        {
            _context.Teachers.Add(new Teacher
            {
                UserId = user.Id,
                Name = name,
                PhoneNumber = phone,
                TeacherCode = "TCH-" + Guid.NewGuid().ToString("N")[..6]
            });
        }

        _context.SaveChanges();
        return RedirectToAction("Login");
    }

    // LOGIN
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || user.PasswordHash != Hash(password))
            return Unauthorized();

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Role", user.Role);

        return user.Role == "Teacher"
            ? RedirectToAction("Dashboard", "Teacher")
            : RedirectToAction("Dashboard", "Student");
    }

    private string Hash(string input)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
}
