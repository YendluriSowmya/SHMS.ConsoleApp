using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHostelManagementSystem
{
    // ========== MODELS ==========

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }
        public double FeePaid { get; set; }
    }

    public class Room
    {
        public int RoomNumber { get; set; }
        public bool IsOccupied { get; set; }
    }

    public class HostelAdmin
    {
        public string Name { get; set; }
    }

    public class Complaint
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }

    public class FeeRecord
    {
        public int StudentId { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PaidOn { get; set; }
    }

    // ========== SERVICES ==========

    public class RoomService
    {
        private List<Room> _rooms = new List<Room>();
        private List<Student> _students = new List<Student>();

        public RoomService()
        {
            // Initialize some rooms
            for (int i = 1; i <= 10; i++)
                _rooms.Add(new Room { RoomNumber = i, IsOccupied = false });
        }

        public bool AllocateRoom(Student student)
        {
            var availableRoom = _rooms.FirstOrDefault(r => !r.IsOccupied);
            if (availableRoom != null)
            {
                student.RoomNumber = availableRoom.RoomNumber;
                availableRoom.IsOccupied = true;
                _students.Add(student);
                return true;
            }
            return false;
        }

        public void ReleaseRoom(int roomNumber)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room != null) room.IsOccupied = false;
        }
    }

    public class ComplaintService
    {
        private List<Complaint> _complaints = new List<Complaint>();

        public void RegisterComplaint(int studentId, string message)
        {
            _complaints.Add(new Complaint
            {
                Id = _complaints.Count + 1,
                StudentId = studentId,
                Message = message,
                Status = "Pending"
            });
        }

        public List<Complaint> GetComplaints() => _complaints;
    }

    public class ReportsService
    {
        public void GenerateRoomReport(List<Room> rooms)
        {
            Console.WriteLine("Room Report:");
            foreach (var room in rooms)
            {
                Console.WriteLine($"Room {room.RoomNumber}: {(room.IsOccupied ? "Occupied" : "Available")}");
            }
        }
    }

    // ========== MAIN PROGRAM ==========

    class Program
    {
        static void Main(string[] args)
        {
            var roomService = new RoomService();
            var complaintService = new ComplaintService();
            var student1 = new Student { Id = 1, Name = "John", FeePaid = 5000 };

            Console.WriteLine("Allocating room...");
            if (roomService.AllocateRoom(student1))
            {
                Console.WriteLine($"Room {student1.RoomNumber} allocated to {student1.Name}.");
            }
            else
            {
                Console.WriteLine("No available rooms.");
            }

            complaintService.RegisterComplaint(student1.Id, "Water leakage in bathroom");
            Console.WriteLine("\nComplaints:");
            foreach (var c in complaintService.GetComplaints())
            {
                Console.WriteLine($"Complaint #{c.Id} from Student {c.StudentId}: {c.Message} - {c.Status}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
