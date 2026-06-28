using Newtonsoft.Json;

namespace ClinicManagementSystem.Models
{
    public class ClinicInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;
        [JsonProperty("adminId")]
        public int AdminId { get; set; }
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("userCount")]
        public int UserCount { get; set; }
        [JsonProperty("appointmentCount")]
        public int AppointmentCount { get; set; }
        [JsonProperty("queueCount")]
        public int QueueCount { get; set; }

        public ClinicCounts Counts { get; set; } = new();
    }

    public class ClinicCounts
    {
        [JsonProperty("doctors")]
        public int Doctors { get; set; }
        [JsonProperty("receptionists")]
        public int Receptionists { get; set; }
        [JsonProperty("patients")]
        public int Patients { get; set; }
        [JsonProperty("appointments")]
        public int Appointments { get; set; }
    }
}
