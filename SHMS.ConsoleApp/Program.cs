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


        if (admins.Count == 0)
            {
                Console.WriteLine("\nNo Hostel Admin Found. Please register.");
                Console.Write("Enter Admin ID: ");
                int adminId = int.Parse(Console.ReadLine());
                Console.Write("Enter Admin Name: ");
                string adminName = Console.ReadLine();
                Console.Write("Enter Contact Number: ");
                string adminContact = Console.ReadLine();

                var admin = new HostelAdmin(adminId, adminName, adminContact);
                admins.Add(admin);
                JsonFileHelper.SaveToFile(admins, "admins.json");
                Console.WriteLine("Hostel Admin registered successfully.\n");
            }

            if (rooms.Count == 0)
            {
                Console.Write("How many rooms do you want to create? ");
                int roomCount = int.Parse(Console.ReadLine());
                for (int i = 0; i < roomCount; i++)
                {
                    Console.Write($"Enter Room Number for Room {i + 1}: ");
                    int roomNum = int.Parse(Console.ReadLine());
                    Console.Write("Enter Capacity: ");
                    int capacity = int.Parse(Console.ReadLine());
                    rooms.Add(new Room(roomNum, capacity));
                }
                JsonFileHelper.SaveToFile(rooms, "rooms.json");
                Console.WriteLine("Rooms created successfully.\n");
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nSMART HOSTEL MANAGEMENT SYSTEM");
                Console.WriteLine("1. Register Student");
                Console.WriteLine("2. Pay Fees");
                Console.WriteLine("3. Register Complaint");
                Console.WriteLine("4. View Complaints");
                Console.WriteLine("5. Update Complaint Status");
                Console.WriteLine("6. Generate Reports");
                Console.WriteLine("7. View Registered Student Information");
                Console.WriteLine("8. Register/View Hostel Admin");
                Console.WriteLine("10. Exit");
                Console.Write("Choose an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine("\n--- Register a New Student ---");

                        Console.Write("Enter Student ID: ");
                        int id = int.Parse(Console.ReadLine());

                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine();

                        Console.Write("Enter Contact Number: ");
                        string contact = Console.ReadLine();

                        Console.Write("Has the student already been allocated a room? (yes/no): ");
                        string allocated = Console.ReadLine().Trim().ToLower();

                        var newStudent = new Student
                        {
                            Id = id,
                            Name = name,
                            ContactNumber = contact
                        };

                        if (allocated == "yes")
                        {
                            Console.Write("Enter Allocated Room Number: ");
                            int roomNum = int.Parse(Console.ReadLine());

                            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNum);
                            if (room != null && !room.IsFull())
                            {
                                room.AddStudent(newStudent);
                                newStudent.RoomNumber = room.RoomNumber;
                                newStudent.IsAllocated = true;
                                room.IsOccupied = true;
                                Console.WriteLine($"Student manually assigned to Room {room.RoomNumber}.");
                            }
                            else
                            {
                                Console.WriteLine("Room is full or doesn't exist. Student not allocated.");
                            }
                        }
                        else
                        {
                            var availableRoom = rooms.FirstOrDefault(r => !r.IsFull());
                            if (availableRoom != null)
                            {
                                availableRoom.AddStudent(newStudent);
                                newStudent.RoomNumber = availableRoom.RoomNumber;
                                newStudent.IsAllocated = true;
                                availableRoom.IsOccupied = true;
                                Console.WriteLine($"Student automatically assigned to Room {availableRoom.RoomNumber}.");
                            }
                            else
                            {
                                Console.WriteLine("No available rooms. Student registered without room.");
                            }
                        }

                        Console.Write("Has the student paid the fee? (yes/no): ");
                        string feeStatus = Console.ReadLine().Trim().ToLower();
                        newStudent.FeePaid = feeStatus == "yes";

                        students.Add(newStudent);

                        if (newStudent.FeePaid)
                        {
                            Console.Write("Enter Amount Paid: ");
                            double amount = double.Parse(Console.ReadLine());
                            var feeRecord = new FeeRecord(newStudent.Id, amount);
                            fees.Add(feeRecord);
                            JsonFileHelper.SaveToFile(fees, "fees.json");
                        }

                        JsonFileHelper.SaveToFile(students, "students.json");
                        JsonFileHelper.SaveToFile(rooms, "rooms.json");

                        Console.WriteLine("Student registration complete.\n");
                        break;

                    case "2":
                        Console.WriteLine("\n--- Pay Fee ---");
                        Console.Write("Enter Student ID: ");
                        int payId = int.Parse(Console.ReadLine());
                        var student = students.FirstOrDefault(s => s.Id == payId);
                        if (student != null)
                        {
                            if (student.FeePaid)
                            {
                                Console.WriteLine("Fee already paid.");
                            }
                            else
                            {
                                Console.Write("Enter Amount Paid: ");
                                double amount = double.Parse(Console.ReadLine());
                                var feeRecord = new FeeRecord(student.Id, amount);
                                fees.Add(feeRecord);
                                student.FeePaid = true;
                                JsonFileHelper.SaveToFile(fees, "fees.json");
                                JsonFileHelper.SaveToFile(students, "students.json");
                                Console.WriteLine("Fee payment recorded successfully.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Student not found.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter Student ID: ");
                        int feeId = int.Parse(Console.ReadLine());
                        Console.Write("Enter Amount Paid: ");
                        double amt = double.Parse(Console.ReadLine());
                        var f = new FeeRecord(feeId, amt);
                        var st = students.FirstOrDefault(s => s.Id == feeId);
                        if (st != null)
                        {
                            f.Student = st;
                            f.IsPaid = true;
                            f.Validate();
                            fees.Add(f);
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
                        foreach (var a in admins)
                            a.DisplayInfo();
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
