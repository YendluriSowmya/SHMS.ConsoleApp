using System;
using SmartHostelManagementSystem.Exceptions;
namespace SmartHostelManagementSystem.Models
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public string StudentId { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; }
        public DateTime DateLogged { get; set; }
        public DateTime ExpectedResolutionDate { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(StudentId))
                throw new MissingComplaintException("Student ID is required for complaint.");

            if (string.IsNullOrWhiteSpace(Issue))
                throw new MissingComplaintException("Complaint issue must be specified.");

            if (DateLogged == default)
                throw new MissingComplaintException("Complaint log date is missing.");

            if (ExpectedResolutionDate == default)
                throw new MissingComplaintException("Expected resolution date is missing.");
        }

    }
}