using SmartHostelManagementSystem.Models;
using System.Text.Json;

namespace SmartHostelManagementSystem.Services;

public class RoomService
{
    public async Task AllocateRoomAsync(Student student, Room room)
    {
        if (room.Occupants.Count >= room.Capacity)
            throw new InvalidOperationException("Room is full.");

        room.AddStudent(student);  // uses Room's AddStudent method
        student.RoomNumber = room.RoomNumber;

        // Create a folder to save data
        Directory.CreateDirectory("data");

        await File.WriteAllTextAsync("data/studentData.json", JsonSerializer.Serialize(student, new JsonSerializerOptions { WriteIndented = true }));
        await File.WriteAllTextAsync("data/roomData.json", JsonSerializer.Serialize(room, new JsonSerializerOptions { WriteIndented = true }));
    }
}
