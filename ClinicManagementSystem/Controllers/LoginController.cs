using ClinicManagementSystem.Models;
using ClinicManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClinicManagementSystem.Controllers
{
    public class LoginController : BaseController
    {
        private readonly AuthService _authService;

        public LoginController(AuthService authService, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, httpContextAccessor)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var jsonData = await _authService.RegisterUserAsync(request);

            if (jsonData == null)
            {
                ViewBag.Error = "Registration failed. Please check your details or clinic code.";
                return View(request);
            }

            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var jsonData = await _authService.AuthenticateUserAsync(request.Email, request.Password);

            if (jsonData == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            var response = JsonConvert.DeserializeObject<LoginResponse>(jsonData);

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }

            _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", response.Token);
            _httpContextAccessor.HttpContext?.Session.SetString("UserRole", response.User.Role);
            _httpContextAccessor.HttpContext?.Session.SetString("UserName", response.User.Name);
            _httpContextAccessor.HttpContext?.Session.SetString("ClinicName", response.User.ClinicName);
            _httpContextAccessor.HttpContext?.Session.SetString("ClinicId", response.User.ClinicId.ToString());

            return response.User.Role.ToLower() switch
            {
                "admin" => RedirectToAction("Index", "Admin"),
                "patient" => RedirectToAction("Index", "Patient"),
                "receptionist" => RedirectToAction("Index", "Receptionist"),
                "doctor" => RedirectToAction("Index", "Doctor"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
