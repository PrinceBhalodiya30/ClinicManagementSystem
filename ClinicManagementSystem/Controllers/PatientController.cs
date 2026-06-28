using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClinicManagementSystem.Controllers
{
    public class PatientController : BaseController
    {
        public PatientController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            AddJwtToken();
            var response = await _client.GetAsync("appointments/my");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
                return View(appointments);
            }

            TempData["Error"] = "Unable to load appointments.";
            return View(new List<Appointment>());
        }

        [HttpGet]
        public IActionResult Book() => View();

        [HttpPost]
        public async Task<IActionResult> Book(BookAppointmentRequest request)
        {
            AddJwtToken();

            if (request.Slot.Contains("AM") || request.Slot.Contains("PM"))
            {
               var time = request.Slot.Replace(" AM", "").Replace(" PM", "");
               if (request.Slot.Contains("PM") && !time.StartsWith("12"))
               {
                   var parts = time.Split(':');
                   time = (int.Parse(parts[0]) + 12).ToString("D2") + ":" + parts[1];
               }
               request.Slot = $"{time}-{time.Split(':')[0]}:15";
            }

            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("appointments", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Appointment booked successfully!";
                return RedirectToAction("Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Failed to book appointment. API Response: {errorContent}";
            return View(request);
        }

        public async Task<IActionResult> Details(int id)
        {
            AddJwtToken();
            var response = await _client.GetAsync($"appointments/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var appointment = JsonConvert.DeserializeObject<Appointment>(json);
                return View(appointment);
            }

            return RedirectToAction("Index");
        }
    }
}
