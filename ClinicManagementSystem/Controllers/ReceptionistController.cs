using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClinicManagementSystem.Controllers
{
    public class ReceptionistController : BaseController
    {
        public ReceptionistController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index(string? date)
        {
            AddJwtToken();
            var targetDate = date ?? DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.SelectedDate = targetDate;

            var response = await _client.GetAsync($"queue?date={targetDate}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var queue = JsonConvert.DeserializeObject<List<QueueEntry>>(json);
                return View(queue);
            }

            TempData["Error"] = "Unable to load queue.";
            return View(new List<QueueEntry>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status, string date)
        {

            AddJwtToken();
            if (status == "in_progress") status = "in-progress";

            var payload = new { status = status };
            var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");
            
            var response = await _client.PatchAsync($"queue/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Status updated successfully.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Failed to update status. API Response: {errorContent}";
            }

            return RedirectToAction("Index", new { date = date });
        }
    }
}
