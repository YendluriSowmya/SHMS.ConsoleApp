using Services;
using SmartHostelManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SHMS.ConsoleApp
{
    public class Program
    {
        static List<Student> students = new();
        static List<Room> rooms = new();
        static List<Complaint> complaints = new();
        static List<FeeRecord> fees = new();

        static async Task Main(string[] args)
        {
            rooms.Add(new Room(101, 2));
            rooms.Add(new Room(102, 3));

            var roomService = new RoomService();
            var complaintManager = new ComplaintManager(complaints);
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
                Console.WriteLine("8. Exit");
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
                        Console.WriteLine("Student registered.");
                        break;

                    case "2":
                        Console.Write("Enter Student ID: ");
                        int sid = int.Parse(Console.ReadLine());
                        Console.Write("Enter Room Number: ");
                        int rno = int.Parse(Console.ReadLine());
                        var student = students.FirstOrDefault(s => s.ID == sid);
                        var room = rooms.FirstOrDefault(r => r.RoomNumber == rno);

                        if (student == null || room == null)
                        {
                            Console.WriteLine("Invalid student or room.");
                            break;
                        }

                        try
                        {
                            await roomService.AllocateRoomAsync(student, room);
                            Console.WriteLine("Room allocated successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
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
                        var stu = students.FirstOrDefault(s => s.ID == feeId);
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

                        await complaintService.RegisterComplaintAsync(complaint);
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
                        complaintService.UpdateComplaintStatus(compToUpdate, status);
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
                                    Console.WriteLine($"- {s.Name} (ID: {s.ID})");
                                break;

                            case "2":
                                var pendings = reportsService.GetComplaintsByStatus("Pending");
                                foreach (var c in pendings)
                                    Console.WriteLine($"- Complaint {c.ComplaintID} by Student {c.StudentID}");
                                break;

                            case "3":
                                var defaulters = reportsService.GetFeeDefaulters();
                                foreach (var s in defaulters)
                                    Console.WriteLine($"- {s.Name} (ID: {s.ID}) has not paid fees.");
                                break;
                        }
                        break;

                    case "8":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

            Console.WriteLine("Exiting SHMS Console App.");
        }
    }
}
