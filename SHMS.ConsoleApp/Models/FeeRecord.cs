namespace SmartHostelManagementSystem.Models;

public class FeeRecord
{
    public int StudentID { get; set; }
    public double AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }

    public FeeRecord() { }

    public FeeRecord(int studentID, double amountPaid)
    {
        StudentID = studentID;
        AmountPaid = amountPaid;
        PaymentDate = DateTime.Now;
    }
}
