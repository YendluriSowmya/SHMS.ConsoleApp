namespace SmartHostelManagementSystem.Models;
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public string StudentId { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; }
        public DateTime DateLogged { get; set; }
        public DateTime ExpectedResolutionDate { get; set; }
    }
}
