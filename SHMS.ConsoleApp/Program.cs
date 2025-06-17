using Services;
using SmartHostelManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace SHMS.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            var rooms = new List<Room>();
            var feeRecords = new List<FeeRecord>();
            var complaints = new List<Complaint>();
            var students = new List<Student>();

            
            var reportsService = new ReportsService(rooms, complaints, feeRecords, students);

            
            ComplaintManager manager = new ComplaintManager();

            Console.WriteLine("1. Register Complaint");
            Console.WriteLine("2. View Complaints");
            Console.WriteLine("3. Update Complaint Status");
            Console.WriteLine("4. Reports");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Enter Student ID: ");
                    string sid = Console.ReadLine();
                    Console.Write("Enter the Issue: ");
                    string desc = Console.ReadLine();

                    manager.RegisterComplaint(new Complaint
                    {
                        StudentId = sid,
                        Issue = desc
                    });

                    Console.WriteLine("Complaint registered successfully!");
                    break;

                case "2":
                    var allComplaints = manager.GetAllComplaints();
                    foreach (var c in allComplaints)
                    {
                        Console.WriteLine($"{c.ComplaintId}: {c.StudentId}, {c.Issue}, Status: {c.Status}, Resolve By: {c.ExpectedResolutionDate.ToShortDateString()}");
                    }
                    break;

                case "3":
                    Console.Write("Enter Complaint ID: ");
                    int cid = int.Parse(Console.ReadLine());
                    Console.Write("Enter New Status: ");
                    string status = Console.ReadLine();
                    manager.UpdateComplaintStatus(cid, status);
                    Console.WriteLine("Complaint status updated.");
                    break;

                case "4":
                    ReportMenu.Launch(reportsService); 
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }
}
