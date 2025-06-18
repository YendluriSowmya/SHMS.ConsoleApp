using SmartHostelManagementSystem.Models;

public class Student : Person
{
    public int RoomNumber { get; set; }
    public bool FeePaid { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public bool IsAllocated { get; set; } = false;

    public Student() { }

    public Student(int id, string name, int roomNumber, bool feePaid)
        : base(id, name)
    {
        RoomNumber = roomNumber;
        FeePaid = feePaid;
    }
}
