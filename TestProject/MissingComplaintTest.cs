
using System;
using SmartHostelManagementSystem.Models;
using SmartHostelManagementSystem.Exceptions;
using Xunit;

namespace TestProject
{
    public class MissingComplaintTest
    {
        [Fact]
        public void Validate_ShouldThrowException_WhenStudentIdIsMissing()
        {
            var complaint = new Complaint
            {
                StudentId = "",
                Issue = "Water leakage",
                DateLogged = DateTime.Now,
                ExpectedResolutionDate = DateTime.Now.AddDays(2)
            };

            Assert.Throws<MissingComplaintException>(() => complaint.Validate());
        }

        [Fact]
        public void Validate_ShouldThrowException_WhenIssueIsMissing()
        {
            var complaint = new Complaint
            {
                StudentId = "123",
                Issue = "",
                DateLogged = DateTime.Now,
                ExpectedResolutionDate = DateTime.Now.AddDays(2)
            };

            Assert.Throws<MissingComplaintException>(() => complaint.Validate());
        }

        [Fact]
        public void Validate_ShouldThrowException_WhenDatesAreMissing()
        {
            var complaint = new Complaint
            {
                StudentId = "123",
                Issue = "Broken fan",
                DateLogged = default,
                ExpectedResolutionDate = default
            };

            Assert.Throws<MissingComplaintException>(() => complaint.Validate());
        }

        [Fact]
        public void Validate_ShouldPass_WhenAllFieldsArePresent()
        {
            var complaint = new Complaint
            {
                StudentId = "123",
                Issue = "Electricity issue",
                DateLogged = DateTime.Now,
                ExpectedResolutionDate = DateTime.Now.AddDays(2)
            };

            complaint.Validate(); // Should not throw
        }
    }
}
