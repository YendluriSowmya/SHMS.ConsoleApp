using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SmartHostelManagementSystem.Models
{
    public class ComplaintManager
    {
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "complaints.json");

        public void RegisterComplaint(Complaint complaint)
        {
            var complaints = LoadComplaints();
            complaint.ComplaintId = complaints.Count + 1;
            complaint.DateLogged = DateTime.Now;
            complaint.ExpectedResolutionDate = DateTime.Now.AddDays(5);
            complaint.Status = "Open";

            complaints.Add(complaint);
            SaveComplaints(complaints);
        }

        public List<Complaint> GetAllComplaints()
        {
            return LoadComplaints();
        }

        public void UpdateComplaintStatus(int id, string newStatus)
        {
            var complaints = LoadComplaints();
            var complaint = complaints.Find(c => c.ComplaintId == id);

            if (complaint == null)
            {
                Console.WriteLine("Complaint not found.");
                return;
            }

            complaint.Status = newStatus;
            SaveComplaints(complaints);
        }

        private List<Complaint> LoadComplaints()
        {
            if (!File.Exists(filePath))
                return new List<Complaint>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Complaint>>(json) ?? new List<Complaint>();
        }

        private void SaveComplaints(List<Complaint> complaints)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); 
            var json = JsonSerializer.Serialize(complaints, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
