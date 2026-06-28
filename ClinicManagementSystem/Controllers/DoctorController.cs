using ClinicManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClinicManagementSystem.Controllers
{
    public class DoctorController : BaseController
    {
        public DoctorController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            AddJwtToken();
            var response = await _client.GetAsync("doctor/queue");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Login");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var queue = JsonConvert.DeserializeObject<List<DoctorQueueItem>>(json);
                return View(queue);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Unable to load today's queue. API Response: {errorContent}";
            return View(new List<DoctorQueueItem>());
        }

        public IActionResult Patients()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddDetails(int id, string patientName)
        {
            AddJwtToken();
            ViewBag.AppointmentId = id;
            ViewBag.PatientName = patientName;

            var response = await _client.GetAsync($"appointments/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var appointment = JsonConvert.DeserializeObject<Appointment>(json);
                return View(appointment);
            }

            return View(new Appointment { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddDetails(int appointmentId, string medicines, string notes, string diagnosis, string tests, string remarks)
        {
            AddJwtToken();
            bool success = true;


            var medicineEntries = medicines.Split(new[] { '\n', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(m => new PrescriptionMedicine { Name = m.Trim(), Dosage = "As directed", Duration = "As prescribed" })
                                           .ToList();

            var prescriptionPayload = new { medicines = medicineEntries, notes = notes };
            var pContent = new StringContent(JsonConvert.SerializeObject(prescriptionPayload), System.Text.Encoding.UTF8, "application/json");
            var pResponse = await _client.PostAsync($"prescriptions/{appointmentId}", pContent);
            if (!pResponse.IsSuccessStatusCode) success = false;


            var reportPayload = new { diagnosis = diagnosis, testRecommended = tests, remarks = remarks };
            var rContent = new StringContent(JsonConvert.SerializeObject(reportPayload), System.Text.Encoding.UTF8, "application/json");
            var rResponse = await _client.PostAsync($"reports/{appointmentId}", rContent);
            if (!rResponse.IsSuccessStatusCode) success = false;

            if (success)
            {
                TempData["Success"] = "Encounter data recorded successfully.";
                return RedirectToAction("Index");
            }

            var pError = await pResponse.Content.ReadAsStringAsync();
            var rError = await rResponse.Content.ReadAsStringAsync();
            TempData["Error"] = $"Error saving data. Prescription: {pError}. Report: {rError}";
            return RedirectToAction("Index");
        }
    }
}
