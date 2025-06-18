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

        public async Task AllocateRoomAsync(Student student, Room room)
        {
            if (room.Occupants.Count >= room.Capacity)
                throw new InvalidOperationException("Room is full.");

            if (!room.HasSpace)
                throw new Exception("Room is already full.");

            if (room.Occupants.Contains(student))
                throw new Exception("Student is already in this room.");

            room.Occupants.Add(student);
            student.RoomNumber = room.RoomNumber;
            student.IsAllocated = true;

            // Save updated lists after allocation
            var students = await LoadStudentsAsync();
            var rooms = await LoadRoomsAsync();

            // Update student in list
            var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent == null)
                students.Add(student);
            else
            {
                existingStudent.RoomNumber = student.RoomNumber;
                existingStudent.FeePaid = student.FeePaid;
            }

            // Update room in list
            var existingRoom = rooms.FirstOrDefault(r => r.RoomNumber == room.RoomNumber);
            if (existingRoom == null)
                rooms.Add(room);
            else
                existingRoom.Occupants = room.Occupants;

            await SaveStudentsAsync(students);
            await SaveRoomsAsync(rooms);
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
            await File.WriteAllTextAsync("rooms.json", json);
        }


        public async Task<List<Room>> LoadRoomsAsync()
        {
            if (File.Exists(roomFilePath))
            {
                var json = await File.ReadAllTextAsync(roomFilePath);
                return JsonSerializer.Deserialize<List<Room>>(json);
            }
            return new List<Room>();
        }

    }
}