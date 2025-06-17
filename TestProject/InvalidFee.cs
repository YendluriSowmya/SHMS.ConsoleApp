using SmartHostelManagementSystem.Models;
using SmartHostelManagementSystem.Exceptions;
using Xunit;

namespace TestProject
{
    public class InvalidFee 
    {
        [Fact]
        public void Validate_ShouldThrowException_WhenAmountPaidIsZero()
        {
            var fee = new FeeRecord(1, 0) { Student = new Student() };

            Assert.Throws<InvalidFeeException>(() => fee.Validate());
        }

        [Fact]
        public void Validate_ShouldThrowException_WhenStudentIDIsInvalid()
        {
            var fee = new FeeRecord(0, 1000) { Student = new Student() };

            Assert.Throws<InvalidFeeException>(() => fee.Validate());
        }

        [Fact]
        public void Validate_ShouldThrowException_WhenStudentIsNull()
        {
            var fee = new FeeRecord(1, 1000) { Student = null };

            Assert.Throws<InvalidFeeException>(() => fee.Validate());
        }

        [Fact]
        public void Validate_ShouldPass_WhenFeeIsValid()
        {
            var fee = new FeeRecord(1, 1000) { Student = new Student() };

            var exception = Record.Exception(() => fee.Validate());

            Assert.Null(exception);
        }
    }
}
