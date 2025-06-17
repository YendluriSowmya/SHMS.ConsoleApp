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
        private readonly List<Student> _students;

        public ReportsService(List<Room> rooms, List<Complaint> complaints, List<FeeRecord> feeRecords, List<Student> students)
        {
            _rooms = rooms ?? new List<Room>();
            _complaints = complaints ?? new List<Complaint>();
            _feeRecords = feeRecords ?? new List<FeeRecord>();
            _students = students ?? new List<Student>();
        }

        public List<Student> GetStudentsByRoom(int roomNumber)
        {
            return _students
                .Where(s => s.RoomNumber == roomNumber)
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
            return _students
                .Where(s => !_feeRecords.Any(f => f.StudentID == s.ID))
                .ToList();
        }

        public void DisplayStudents(List<Student> students)
        {
            if (students == null || students.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}");
            }
        }

        public void DisplayComplaints(List<Complaint> complaints)
        {
            if (complaints == null || complaints.Count == 0)
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
