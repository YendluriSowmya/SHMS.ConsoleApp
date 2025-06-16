using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHostelManagementSystem.Models;

public class ComplaintManager
    {
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "complaints.json");

        public async Task RegisterComplaintAsync(Complaint complaint)
        {
            var complaints = await LoadComplaintsAsync();
            complaint.ComplaintId = complaints.Count + 1;
            complaint.DateLogged = DateTime.Now;
            complaint.ExpectedResolutionDate = DateTime.Now.AddDays(5);
            complaint.Status = "Open";

            complaints.Add(complaint);
            await SaveComplaintsAsync(complaints);
        }

        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await LoadComplaintsAsync();
        }

        public async Task UpdateComplaintStatusAsync(int id, string newStatus)
        {
            var complaints = await LoadComplaintsAsync();
            var complaint = complaints.Find(c => c.ComplaintId == id);

            if (complaint == null)
            {
                Console.WriteLine("Complaint not found.");
                return;
            }

            complaint.Status = newStatus;
            await SaveComplaintsAsync(complaints);
        }

        private async Task<List<Complaint>> LoadComplaintsAsync()
        {
            if (!File.Exists(filePath))
                return new List<Complaint>();

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Complaint>>(json) ?? new List<Complaint>();
        }

        private async Task SaveComplaintsAsync(List<Complaint> complaints)
        {
            var json = JsonSerializer.Serialize(complaints, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
