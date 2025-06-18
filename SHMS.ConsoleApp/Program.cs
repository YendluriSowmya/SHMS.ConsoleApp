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
                Console.WriteLine("9. Exit");
                Console.Write("Choose an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter Student ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine();
                        students.Add(new Student(id, name, 0, false));
                        JsonFileHelper.SaveToFile(students, "students.json");
                        Console.WriteLine("Student registered.");
                        break;

                    case "2":


                    
                        // Display only unallocated students
                        Console.WriteLine("\nAvailable Students:");
                        foreach (var s in students.Where(s => !s.IsAllocated))
                            Console.WriteLine($"ID: {s.Id}, Name: {s.Name}");

                        // Display only rooms that are not occupied
                        Console.WriteLine("\nAvailable Rooms:");
                        foreach (var r in rooms.Where(r => !r.IsOccupied))
                            Console.WriteLine($"Room No: {r.RoomNumber}");

                        Console.Write("\nEnter Student ID: ");
                        int sid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Room Number: ");
                        int rno = int.Parse(Console.ReadLine());

                        var student = students.FirstOrDefault(s => s.Id == sid);
                        var room = rooms.FirstOrDefault(r => r.RoomNumber == rno);

                        if (student == null)
                        {
                            Console.WriteLine(" Student not found.");
                            break;
                        }

                        if (student.IsAllocated)
                        {
                            Console.WriteLine("Student is already allocated a room.");
                            break;
                        }

                        if (room == null)
                        {
                            Console.WriteLine(" Room not found.");
                            break;
                        }

                        if (room.IsOccupied)
                        {
                            Console.WriteLine("Room is already allocated to someone else.");
                            break;
                        }

                        // Allocate room
                        room.IsOccupied = true;
                        student.IsAllocated = true;
                        student.RoomNumber = rno;  // Save the allocated room to the student

                        Console.WriteLine("✅ Room allocated successfully.");

                        await roomService.SaveRoomsAsync(rooms);
                        await roomService.SaveStudentsAsync(students);
                        break;


                    case "3":
                        Console.Write("Enter Student ID: ");
                        int feeId = int.Parse(Console.ReadLine());
                        Console.Write("Enter Amount: ");
                        double amount = double.Parse(Console.ReadLine());

                        if (amount < 0)
                        {
                            Console.WriteLine("Fee amount cannot be negative.");
                            break;
                        }

                        fees.Add(new FeeRecord(feeId, amount));
                        JsonFileHelper.SaveToFile(fees, "fees.json");
                        var stu = students.FirstOrDefault(s => s.Id == feeId);
                        if (stu != null) stu.FeePaid = true;

                        Console.WriteLine("Fee payment recorded.");
                        break;

                    case "4":
                        Console.Write("Enter Student ID: ");
                        int compId = int.Parse(Console.ReadLine());
                        Console.Write("Enter Complaint Description: ");
                        string issue = Console.ReadLine();

                        var complaint = new Complaint
                        {
                            ComplaintID = complaints.Count + 1,
                            StudentID = compId,
                            Issue = issue,
                            RaisedOn = DateTime.Now,
                            Status = "Pending"
                        };

                        await complaintManager.RegisterComplaintAsync(complaint);
                        complaints.Add(complaint);
                        JsonFileHelper.SaveToFile(complaints, "complaints.json");
                        Console.WriteLine("Complaint registered.");
                        break;

                    case "5":
                        var all = complaintManager.GetAllComplaints();
                        foreach (var c in all)
                        {
                            Console.WriteLine($"ID: {c.ComplaintID}, Student: {c.StudentID}, Issue: {c.Issue}, Status: {c.Status}");
                        }
                        break;

                    case "6":
                        Console.Write("Enter Complaint ID: ");
                        int compToUpdate = int.Parse(Console.ReadLine());
                        Console.Write("Enter New Status: ");
                        string status = Console.ReadLine();
                        complaintManager.UpdateComplaintStatus(compToUpdate, status);
                        Console.WriteLine("Complaint status updated.");
                        break;

                    case "7":
                        Console.WriteLine("1. Students in Room");
                        Console.WriteLine("2. Pending Complaints");
                        Console.WriteLine("3. Fee Defaulters");
                        Console.Write("Choose Report Option: ");
                        string reportOption = Console.ReadLine();

                        switch (reportOption)
                        {
                            case "1":
                                Console.Write("Enter Room Number: ");
                                int rn = int.Parse(Console.ReadLine());
                                var studs = reportsService.GetStudentsByRoom(rn);
                                foreach (var s in studs)
                                    Console.WriteLine($"- {s.Name} (ID: {s.Id})");
                                break;

                            case "2":
                                var pendings = reportsService.GetComplaintsByStatus("Pending");
                                foreach (var c in pendings)
                                    Console.WriteLine($"- Complaint {c.ComplaintID} by Student {c.StudentID}");
                                break;

                            case "3":
                                var defaulters = reportsService.GetFeeDefaulters();
                                foreach (var s in defaulters)
                                    Console.WriteLine($"- {s.Name} (ID: {s.Id}) has not paid fees.");
                                break;
                        }
                        break;

                    case "8":
                        Console.WriteLine("\n--- Complete Student Information ---");
                        foreach (var s in students)
                        {
                            Console.WriteLine($"\nStudent ID: {s.Id}");
                            Console.WriteLine($"Name: {s.Name}");

                            // Room info
                            var roomAllocated = rooms.FirstOrDefault(r => r.IsOccupied && r.RoomNumber == s.RoomNumber);
                            Console.WriteLine("Room Number: " + (roomAllocated != null ? roomAllocated.RoomNumber.ToString() : "Not Allocated"));

                            // Fee status
                            var feeRecord = fees.FirstOrDefault(f => f.StudentID == s.Id);
                            Console.WriteLine("Fee Status: " + (feeRecord != null ? "Paid" : "Not Paid"));

                            // Complaints
                            var studentComplaints = complaints.Where(c => c.StudentID == s.Id).ToList();
                            if (studentComplaints.Any())
                            {
                                Console.WriteLine("Complaints:");
                                foreach (var c in studentComplaints)
                                {
                                    Console.WriteLine($" - {c.Issue} (Status: {c.Status})");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Complaints: None");
                            }
                        }
                        break;


                    case "9":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

            // Save changes on exit
            await roomService.SaveStudentsAsync(students);
            await roomService.SaveRoomsAsync(rooms);

            JsonFileHelper.SaveToFile(complaints, "complaints.json");
            JsonFileHelper.SaveToFile(fees, "fees.json");
            JsonFileHelper.SaveToFile(students, "students.json");



            Console.WriteLine("Exiting SHMS Console App.");


        }
    }
}