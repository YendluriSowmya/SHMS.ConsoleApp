public static class ReportMenu
{
    public static void Launch(ReportsService reportService)
    {
        Console.WriteLine("1. View students in a room");
        Console.WriteLine("2. View complaints by status");
        Console.WriteLine("3. View fee defaulters");

        Console.Write("Choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter room number: ");
                int roomNo = int.Parse(Console.ReadLine());
                var students = reportService.GetStudentsByRoom(roomNo);
                reportService.DisplayStudents(students);
                break;

            case "2":
                Console.Write("Enter complaint status: ");
                string status = Console.ReadLine();
                var complaints = reportService.GetComplaintsByStatus(status);
                reportService.DisplayComplaints(complaints);
                break;

            case "3":
                var defaulters = reportService.GetFeeDefaulters();
                reportService.DisplayStudents(defaulters);
                break;
        }
    }
}
