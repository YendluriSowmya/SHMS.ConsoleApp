using SmartHostelManagementSystem.Models;
using System.Text.Json;
using System.Linq;

namespace SmartHostelManagementSystem.Services
{
    public class RoomService
    {
        private readonly string roomFilePath = "rooms.json";
        private readonly string studentFilePath = "students.json";

        // ✅ Allocate a student to a room
        public async Task AllocateRoomAsync(int studentId, int roomNumber, List<Student> students, List<Room> rooms)
        {
            var student = students.FirstOrDefault(s => s.Id == studentId);
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            if (room == null)
            {
                Console.WriteLine("Room not found.");
                return;
            }

            if (student.IsAllocated)
            {
                Console.WriteLine("Student is already allocated to a room.");
                return;
            }

            if (room.IsFull())
            {
                Console.WriteLine("Room is full. Cannot allocate.");
                return;
            }

            // Perform allocation
            room.AddStudent(student);
            student.RoomNumber = room.RoomNumber;
            student.IsAllocated = true;
            room.IsOccupied = true;

            Console.WriteLine($"Room {room.RoomNumber} allocated to {student.Name} (ID: {student.Id})");

            // Save updated lists
            await SaveStudentsAsync(students);
            await SaveRoomsAsync(rooms);
        }

        // ✅ Save students to JSON
        public async Task SaveStudentsAsync(List<Student> students)
        {
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(studentFilePath, json);
        }

        // ✅ Save rooms to JSON
        public async Task SaveRoomsAsync(List<Room> rooms)
        {
            var json = JsonSerializer.Serialize(rooms, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(roomFilePath, json);
        }

        // ✅ Load or auto-create rooms
        public async Task<List<Room>> LoadRoomsAsync()
        {
            if (!File.Exists(roomFilePath) || new FileInfo(roomFilePath).Length == 0)
            {
                var defaultRooms = new List<Room>
                {
                    new Room(101, 2),
                    new Room(102, 3),
                    new Room(103, 1)
                };
                await SaveRoomsAsync(defaultRooms);
                Console.WriteLine("Default rooms created.");
                return defaultRooms;
            }

            var json = await File.ReadAllTextAsync(roomFilePath);
            return JsonSerializer.Deserialize<List<Room>>(json) ?? new List<Room>();
        }

        // Optional: load students from file
        public async Task<List<Student>> LoadStudentsAsync()
        {
            if (File.Exists(studentFilePath))
            {
                var json = await File.ReadAllTextAsync(studentFilePath);
                return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            }
            return new List<Student>();
        }
    }
}
