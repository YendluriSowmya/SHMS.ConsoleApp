using System;
using SmartHostelManagementSystem.Exceptions;

namespace SmartHostelManagementSystem.Models
{
    public class Complaint
    {
        public int ComplaintID { get; set; } // Use capital ID to match everywhere
        public int StudentID { get; set; }   // Should be int, not string
        public string Issue { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RaisedOn { get; set; }
        public DateTime ExpectedResolutionDate { get; set; }

        public void Validate()
        {
            if (StudentID <= 0)
                throw new MissingComplaintException("Valid Student ID is required.");

            if (string.IsNullOrWhiteSpace(Issue))
                throw new MissingComplaintException("Complaint issue must be specified.");

            if (RaisedOn == default)
                throw new MissingComplaintException("Complaint log date is missing.");

            if (ExpectedResolutionDate == default)
                throw new MissingComplaintException("Expected resolution date is missing.");
        }
    }
}