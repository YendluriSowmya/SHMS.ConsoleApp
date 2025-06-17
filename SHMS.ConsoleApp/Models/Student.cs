    namespace SmartHostelManagementSystem.Models;

    public class Student : Person
    {
        public int RoomNumber { get; set; }
        public bool FeePaid { get; set; }

        public Student() { }

        public Student(int id, string name, int roomNumber, bool feePaid) : base(id, name)
        {
            RoomNumber = roomNumber;
            FeePaid = feePaid;
        }
    }
