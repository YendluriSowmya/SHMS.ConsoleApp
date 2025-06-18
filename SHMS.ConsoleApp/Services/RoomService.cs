using SmartHostelManagementSystem.Models;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SmartHostelManagementSystem.Services
{
    public class RoomService
    {
        private readonly string roomFilePath = "rooms.json";
        private readonly string _studentFilePath = Path.Combine("data", "students.txt");
        private readonly string _roomFilePath = Path.Combine("data", "rooms.txt");

        public RoomService()
        {
            Directory.CreateDirectory("data");
        }

        // ✅ NEW METHOD to match Program.cs call
        public async Task AllocateRoomAsync(int studentId, int roomNumber, List<Student> students, List<Room> rooms)
        {
            var student = students.FirstOrDefault(s => s.Id == studentId);
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);

            if (student == null)
                throw new ArgumentException("Student not found.");
            if (room == null)
                throw new ArgumentException("Room not found.");
            if (room.Occupants.Count >= room.Capacity || !room.HasSpace)
                throw new InvalidOperationException("Room is full.");
            if (room.Occupants.Any(s => s.Id == studentId))
                throw new Exception("Student is already in this room.");

            // Add student to room
            room.Occupants.Add(student);
            student.RoomNumber = room.RoomNumber;
            student.IsAllocated = true;
            room.IsOccupied = true;

            // Save to files
            await SaveStudentsAsync(students);
            await SaveRoomsAsync(rooms);

            Console.WriteLine("Room allocated successfully.");
        }

        public async Task SaveStudentsAsync(List<Student> students)
        {
            using var writer = new StreamWriter(_studentFilePath, false, Encoding.UTF8);
            foreach (var s in students)
            {
                string line = $"{s.Id},{s.Name},{s.RoomNumber},{s.FeePaid}";
                await writer.WriteLineAsync(line);
            }
        }

        public async Task<List<Student>> LoadStudentsAsync()
        {
            var students = new List<Student>();

            if (!File.Exists(_studentFilePath))
                return students;

            using var reader = new StreamReader(_studentFilePath, Encoding.UTF8);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var parts = line?.Split(',');

                if (parts?.Length == 4)
                {
                    students.Add(new Student(
                        id: int.Parse(parts[0]),
                        name: parts[1],
                        roomNumber: int.Parse(parts[2]),
                        feePaid: bool.Parse(parts[3])
                    ));
                }
            }

            return students;
        }

        public async Task SaveRoomsAsync(List<Room> rooms)
        {
            var json = JsonSerializer.Serialize(rooms, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(roomFilePath, json);
        }

        public async Task<List<Room>> LoadRoomsAsync()
        {
            if (File.Exists(roomFilePath))
            {
                var json = await File.ReadAllTextAsync(roomFilePath);
                return JsonSerializer.Deserialize<List<Room>>(json) ?? new List<Room>();
            }
            return new List<Room>();
        }
    }
}
