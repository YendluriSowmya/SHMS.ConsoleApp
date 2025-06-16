namespace SmartHostelManagementSystem.Models;

public class Complaint
{
    public int ComplaintID { get; set; }
    public int StudentID { get; set; }
    public string Issue { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime RaisedOn { get; set; }

    public Complaint() { }

    public Complaint(int complaintID, int studentID, string issue)
    {
        ComplaintID = complaintID;
        StudentID = studentID;
        Issue = issue;
        RaisedOn = DateTime.Now;
    }

    public void Resolve()
    {
        Status = "Resolved";
    }
}
