namespace ClinicManagementSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ClinicId { get; set; }
        [Newtonsoft.Json.JsonProperty("appointmentDate")]
        public string Date { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("timeSlot")]
        public string Slot { get; set; } = string.Empty;
        public string Status { get; set; } = "queued";
        public DateTime CreatedAt { get; set; }
        public QueueEntry? QueueEntry { get; set; }
        [Newtonsoft.Json.JsonProperty("prescription")]
        public Prescription? Prescription { get; set; }

        [Newtonsoft.Json.JsonProperty("report")]
        public Report? Report { get; set; }
    }

    public class QueueEntry
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }
        
        [Newtonsoft.Json.JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }
        
        [Newtonsoft.Json.JsonProperty("tokenNumber")]
        public int TokenNumber { get; set; }
        
        [Newtonsoft.Json.JsonProperty("status")]
        public string Status { get; set; } = "waiting"; 
        
        [Newtonsoft.Json.JsonProperty("queueDate")]
        public string Date { get; set; } = string.Empty;
    }

    public class BookAppointmentRequest
    {
        [Newtonsoft.Json.JsonProperty("appointmentDate")]
        public string Date { get; set; } = string.Empty; 
        
        [Newtonsoft.Json.JsonProperty("timeSlot")]
        public string Slot { get; set; } = string.Empty; 
    }

    public class PrescriptionMedicine
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("dosage")]
        public string Dosage { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("duration")]
        public string Duration { get; set; } = string.Empty;
    }

    public class Prescription
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        
        [Newtonsoft.Json.JsonProperty("medicines")]
        public List<PrescriptionMedicine> Medicines { get; set; } = new();
        
        [Newtonsoft.Json.JsonProperty("notes")]
        public string Notes { get; set; } = string.Empty;
    }

    public class Report
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }
        
        [Newtonsoft.Json.JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }
        
        [Newtonsoft.Json.JsonProperty("diagnosis")]
        public string Diagnosis { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("testRecommended")]
        public string Tests { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("remarks")]
        public string Remarks { get; set; } = string.Empty;
    }

    public class DoctorQueueItem
    {
        [Newtonsoft.Json.JsonProperty("tokenNumber")]
        public int TokenNumber { get; set; }
        
        [Newtonsoft.Json.JsonProperty("patientName")]
        public string PatientName { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonProperty("appointmentId")]
        public int AppointmentId { get; set; }
        
        [Newtonsoft.Json.JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
    }
}
