using Services;
using SmartHostelManagementSystem.Models;
using SmartHostelManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHMS.ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var roomService = new RoomService();
            var students = JsonFileHelper.LoadFromFile<Student>("students.json");
            var rooms = await roomService.LoadRoomsAsync();
            var complaints = JsonFileHelper.LoadFromFile<Complaint>("complaints.json");
            var fees = JsonFileHelper.LoadFromFile<FeeRecord>("fees.json");
            var admins = JsonFileHelper.LoadFromFile<HostelAdmin>("admins.json");
            var complaintManager = new ComplaintManager();
            var reportsService = new ReportsService(rooms, complaints, fees, students);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nSMART HOSTEL MANAGEMENT SYSTEM");
                Console.WriteLine("1. Register Student");
                Console.WriteLine("2. Allocate Room");
                Console.WriteLine("3. Pay Fees");
                Console.WriteLine("4. Register Complaint");
                Console.WriteLine("5. View Complaints");
                Console.WriteLine("6. Update Complaint Status");
                Console.WriteLine("7. Generate Reports");
                Console.WriteLine("8. View Registered Student Information");
                Console.WriteLine("9. Register/View Hostel Admin");
                Console.WriteLine("10. Exit");
                Console.Write("Choose an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter Student ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter Contact Number: ");
                        string contact = Console.ReadLine();
                        students.Add(new Student(id, name, 0, false) { ContactNumber = contact });
                        JsonFileHelper.SaveToFile(students, "students.json");
                        Console.WriteLine("Student registered.");
                        break;

                    case "2":
                        Console.WriteLine("\nAvailable Students:");
                        foreach (var s in students.Where(s => !s.IsAllocated))
                            Console.WriteLine($"ID: {s.Id}, Name: {s.Name}");

                        Console.WriteLine("\nAvailable Rooms:");
                        foreach (var r in rooms.Where(r => r.HasSpace))
                            Console.WriteLine($"Room No: {r.RoomNumber}, Capacity: {r.Capacity - r.Occupants.Count} remaining");

                        Console.Write("Enter Student ID: ");
                        int sid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Room Number: ");
                        int rno = int.Parse(Console.ReadLine());
                        await roomService.AllocateRoomAsync(sid, rno, students, rooms);
                        break;

                    case "3":
                        Console.Write("Enter Student ID: ");
                        int feeId = int.Parse(Console.ReadLine());
                        Console.Write("Enter Amount Paid: ");
                        double amount = double.Parse(Console.ReadLine());
                        var fee = new FeeRecord(feeId, amount);
                        var student = students.FirstOrDefault(s => s.Id == feeId);
                        if (student != null)
                        {
                            fee.Student = student;
                            fee.IsPaid = true;
                            fee.Validate();
                            fees.Add(fee);
                            JsonFileHelper.SaveToFile(fees, "fees.json");
                            Console.WriteLine("Fee payment recorded.");
                        }
                        else
                        {
                            Console.WriteLine("Student not found.");
                        }
                        break;

                    case "4":
                        Console.Write("Enter Complaint ID: ");
                        int cid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Student ID: ");
                        int compSid = int.Parse(Console.ReadLine());
                        Console.Write("Describe the Issue: ");
                        string issue = Console.ReadLine();
                        Console.Write("Enter Expected Resolution Date (yyyy-mm-dd): ");
                        DateTime exp = DateTime.Parse(Console.ReadLine());

                        var complaint = new Complaint
                        {
                            ComplaintID = cid,
                            StudentID = compSid,
                            Issue = issue,
                            Status = "Pending",
                            RaisedOn = DateTime.Now,
                            ExpectedResolutionDate = exp
                        };

                        complaint.Validate();
                        complaints.Add(complaint);
                        JsonFileHelper.SaveToFile(complaints, "complaints.json");
                        Console.WriteLine("Complaint registered.");
                        break;

                    case "5":
                        Console.Write("Enter Status to Filter (e.g., Pending, Resolved): ");
                        string status = Console.ReadLine();
                        var filtered = complaints.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                        foreach (var comp in filtered)
                            Console.WriteLine($"Complaint ID: {comp.ComplaintID}, Issue: {comp.Issue}, Student ID: {comp.StudentID}");
                        break;

                    case "6":
                        Console.Write("Enter Complaint ID: ");
                        int compToUpdate = int.Parse(Console.ReadLine());
                        Console.Write("Enter New Status (e.g., Resolved, In Progress): ");
                        string newStatus = Console.ReadLine();
                        complaintManager.UpdateComplaintStatus(compToUpdate, newStatus);
                        JsonFileHelper.SaveToFile(complaints, "complaints.json");
                        Console.WriteLine("Complaint status updated.");
                        break;

                    case "7":
                        reportsService.GenerateReport();
                        break;

                    case "8":
                        foreach (var s in students)
                        {
                            Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Contact: {s.ContactNumber}, Room: {s.RoomNumber}, Fee Paid: {s.FeePaid}");
                        }
                        break;

                    case "9":
                        Console.WriteLine("\n--- Hostel Admin Registration ---");
                        Console.Write("Enter Admin ID: ");
                        int adminId = int.Parse(Console.ReadLine());
                        Console.Write("Enter Admin Name: ");
                        string adminName = Console.ReadLine();
                        Console.Write("Enter Admin Contact Number: ");
                        string adminContact = Console.ReadLine();

                        var admin = new HostelAdmin(adminId, adminName, adminContact);
                        admins.Add(admin);
                        JsonFileHelper.SaveToFile(admins, "admins.json");

                        Console.WriteLine("Admin Registered Successfully.");
                        admin.DisplayInfo();
                        break;

                    case "10":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
