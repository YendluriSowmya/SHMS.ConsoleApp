using ComplaintService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHMS.ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
<<<<<<< HEAD
            int[] scores = [213, 414, 44, 1223, 41, 44, 14];

            IEnumerable<int> scoreQuery =
                from score in scores
                where score > 80
                select score;   
=======
            ComplaintManager manager = new ComplaintManager();
            Console.WriteLine("1. Register Complaint\n2. View Complaints\n3. Update Complaint Status");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Enter Student ID: ");
                    string sid = Console.ReadLine();
                    Console.Write("Enter the Issue: ");
                    string desc = Console.ReadLine();
                    await manager.RegisterComplaintAsync(new Complaint
                    {
                        StudentId = sid,
                        Issue = desc
                    });
                    Console.WriteLine("Complaint registered successfully!");
                    break;

                case "2":
                    var complaints = await manager.GetAllComplaintsAsync();
                    foreach (var c in complaints)
                    {
                        Console.WriteLine($"{c.ComplaintId}: {c.StudentId}, {c.Issue}, Status: {c.Status}, Resolve By: {c.ExpectedResolutionDate.ToShortDateString()}");
                    }
                    break;

                case "3":
                    Console.Write("Enter Complaint ID: ");
                    int cid = int.Parse(Console.ReadLine());
                    Console.Write("Enter New Status: ");
                    string status = Console.ReadLine();
                    await manager.UpdateComplaintStatusAsync(cid, status);
                    Console.WriteLine("Complaint status updated.");
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
>>>>>>> f9f008f96566d23498dede77fe485db1f6d80416
        }
    }

}
