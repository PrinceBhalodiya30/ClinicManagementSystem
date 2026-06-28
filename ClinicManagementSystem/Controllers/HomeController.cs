using System.Diagnostics;
using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ClinicManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("UserRole")?.ToLower();

            return role switch
            {
                "admin" => RedirectToAction("Index", "Admin"),
                "patient" => RedirectToAction("Index", "Patient"),
                "receptionist" => RedirectToAction("Index", "Receptionist"),
                "doctor" => RedirectToAction("Index", "Doctor"),
                _ => RedirectToAction("Login", "Login")
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
