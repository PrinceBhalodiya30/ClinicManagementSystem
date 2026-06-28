using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace ClinicManagementSystem.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            AddJwtToken();
            var response = await _client.GetAsync("admin/clinic");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var clinic = JsonConvert.DeserializeObject<ClinicInfo>(json);

                var usersResponse = await _client.GetAsync("admin/users");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var usersJson = await usersResponse.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<UserInfo>>(usersJson);
                    
                    if (clinic != null && users != null)
                    {
                        clinic.Counts.Doctors = users.Count(u => u.Role.ToLower() == "doctor");
                        clinic.Counts.Receptionists = users.Count(u => u.Role.ToLower() == "receptionist");
                        clinic.Counts.Patients = users.Count(u => u.Role.ToLower() == "patient");
                        clinic.Counts.Appointments = clinic.AppointmentCount; 
                    }
                }

                return View(clinic);
            }

            TempData["Error"] = "Unable to load clinic info.";
            return View(new ClinicInfo());
        }

        public async Task<IActionResult> Users()
        {
            AddJwtToken();
            var response = await _client.GetAsync("admin/users");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserInfo>>(json);
                return View(users);
            }

            TempData["Error"] = "Unable to load users.";
            return View(new List<UserInfo>());
        }

        [HttpGet]
        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInfo user, string password)
        {
            AddJwtToken();
            var payload = new
            {
                name = user.Name,
                email = user.Email,
                password = password,
                role = user.Role.ToLower(),
                phone = user.Phone
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("admin/users", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "User created successfully.";
                return RedirectToAction("Users");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Failed to create user. API Response: {errorContent}";
            return View(user);
        }
    }
}
