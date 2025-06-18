using SmartHostelManagementSystem.Exceptions;
using System;

namespace SmartHostelManagementSystem.Models;

public class FeeRecord
{
    public int StudentID { get; set; }
    public double AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }

    public bool IsPaid { get; set; }
    public Student Student { get; set; }


    public FeeRecord() { }

    public FeeRecord(int studentID, double amountPaid)
    {
        StudentID = studentID;
        AmountPaid = amountPaid;
        PaymentDate = DateTime.Now;
    }

    public void Validate()
    {
        if (StudentID <= 0)
            throw new InvalidFeeException("Student ID is invalid.");

        if (AmountPaid <= 0)
            throw new InvalidFeeException("Amount paid must be greater than zero.");

        if (Student == null)
            throw new InvalidFeeException("Associated student is missing.");
    }
}
