using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace EMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =======================
        // REGISTER (GET)
        // =======================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // =======================
        // REGISTER (POST)
        // =======================
        [HttpPost]
        public IActionResult Register(
            string email,
            string password,
            string role,
            string name,
            string phone)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(role) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.Error = "All fields are required";
                return View();
            }

            // Check email uniqueness
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email already exists";
                return View();
            }

            // Create user
            var user = new User
            {
                Email = email,
                PasswordHash = Hash(password),
                Role = role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Create related profile
            if (role == "Student")
            {
                var student = new Student
                {
                    UserId = user.Id,
                    StudentCode = GenerateStudentCode(),
                    Name = name,
                    PhoneNumber = phone
                };

                _context.Students.Add(student);
            }
            else if (role == "Teacher")
            {
                var teacher = new Teacher
                {
                    UserId = user.Id,
                    TeacherCode = GenerateTeacherCode(),
                    Name = name,
                    PhoneNumber = phone
                };

                _context.Teachers.Add(teacher);
            }
            else
            {
                ViewBag.Error = "Invalid role selected";
                return View();
            }

            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        // =======================
        // LOGIN (GET)
        // =======================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =======================
        // LOGIN (POST)
        // =======================
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || user.PasswordHash != Hash(password))
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            // Save session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);

            return user.Role == "Teacher"
                ? RedirectToAction("Dashboard", "Class")
                : RedirectToAction("Dashboard", "Student");
        }

        // =======================
        // LOGOUT
        // =======================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // =======================
        // HELPERS
        // =======================
        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input))
            );
        }

        private string GenerateStudentCode()
        {
            return "STD-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
        }

        private string GenerateTeacherCode()
        {
            return "TCH-" + Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
