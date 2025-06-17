using System;
using System.Collections.Generic;
using System.Linq;
using SmartHostelManagementSystem.Models;

namespace Services
{
    public class ReportsService
    {
        private readonly List<Room> _rooms;
        private readonly List<Complaint> _complaints;
        private readonly List<FeeRecord> _feeRecords;

        public ReportsService(List<Room> rooms, List<Complaint> complaints, List<FeeRecord> feeRecords)
        {
            _rooms = rooms;
            _complaints = complaints;
            _feeRecords = feeRecords;
        }

        public List<Student> GetStudentsByRoom(int roomNumber)
        {
            return _rooms
                .Where(r => r.RoomNumber == roomNumber)
                .SelectMany(r => r.Occupants)
                .ToList();
        }

        public List<Complaint> GetComplaintsByStatus(string status)
        {
            return _complaints
                .Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Student> GetFeeDefaulters()
        {
            var defaulters = _feeRecords
                .Where(f => !f.IsPaid)
                .Select(f => f.Student)
                .ToList();

            return defaulters;
        }

        public void DisplayStudents(List<Student> students)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id}, Name: {student.Name}");
            }
        }

        public void DisplayComplaints(List<Complaint> complaints)
        {
            if (complaints.Count == 0)
            {
                Console.WriteLine("No complaints found.");
                return;
            }

            foreach (var c in complaints)
            {
                Console.WriteLine($"ID: {c.ComplaintId}, Student: {c.StudentId}, Status: {c.Status}, Issue: {c.Issue}");
            }
        }
    }
}
