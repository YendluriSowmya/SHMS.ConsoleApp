using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHostelManagementSystem.Models
{
    public class ComplaintManager
    {
        private readonly string filePath = Path.Combine(AppContext.BaseDirectory, "data", "complaints.json");

        public ComplaintManager()
        {
            // Ensure the data folder exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        }

        // Method 1: Register a new complaint asynchronously
        public async Task RegisterComplaintAsync(Complaint complaint)
        {
            var complaints = await LoadComplaintsAsync();
            complaint.ComplaintID = complaints.Count + 1;
            complaint.RaisedOn = DateTime.Now;
            complaint.Status = "Pending";

            complaints.Add(complaint);
            await SaveComplaintsAsync(complaints);
        }

        // Method 2: Return all complaints (called synchronously in Program.cs)
        public List<Complaint> GetAllComplaints()
        {
            return LoadComplaintsAsync().Result;
        }

        // Method 3: Update complaint status (called synchronously in Program.cs)
        public void UpdateComplaintStatus(int id, string newStatus)
        {
            var complaints = LoadComplaintsAsync().Result;
            var complaint = complaints.Find(c => c.ComplaintID == id);

            if (complaint == null)
            {
                Console.WriteLine("Complaint not found.");
                return;
            }

            complaint.Status = newStatus;
            SaveComplaintsAsync(complaints).Wait();
        }

        // Helper: Load from complaints.json (async)
        private async Task<List<Complaint>> LoadComplaintsAsync()
        {
            if (!File.Exists(filePath))
                return new List<Complaint>();

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Complaint>>(json) ?? new List<Complaint>();
        }

        // Helper: Save to complaints.json (async)
        private async Task SaveComplaintsAsync(List<Complaint> complaints)
        {
            var json = JsonSerializer.Serialize(complaints, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}