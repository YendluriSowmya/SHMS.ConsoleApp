namespace SmartHostelManagementSystem.Models;

public class HostelAdmin : Person
{
    public string ContactNumber { get; set; }

    public HostelAdmin() { }

    public HostelAdmin(int id, string name, string contactNumber) : base(id, name)
    {
        ContactNumber = contactNumber;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Admin: {Name}, Contact: {ContactNumber}");
    }
}
