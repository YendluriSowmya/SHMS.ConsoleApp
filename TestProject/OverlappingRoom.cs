using System;
using SmartHostelManagementSystem.Models;
namespace TestProject
{
    public class RoomTests
    {
        [Fact]
        public void AddStudent_ShouldThrowException_WhenRoomIsFull()
        {
            // Arrange
            var room = new Room(roomNumber: 101, capacity: 2);

            var student1 = new Student(1, "John Doe", 101, true);
            var student2 = new Student(2, "Jane Smith", 101, true);
            var student3 = new Student(3, "Mike Johnson", 101, true); // Will cause overlap

            // Act
            room.AddStudent(student1);
            room.AddStudent(student2);

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(() => room.AddStudent(student3));
            Assert.Equal("Room is already full.", exception.Message);
        }

        [Fact]
        public void AddStudent_ShouldAddStudent_WhenRoomHasCapacity()
        {
            // Arrange
            var room = new Room(roomNumber: 102, capacity: 2);
            var student = new Student(4, "Alice Cooper", 102, true);

            // Act
            room.AddStudent(student);

            // Assert
            Assert.Single(room.Occupants);
            Assert.Equal("Alice Cooper", room.Occupants[0].Name);
        }
    }
}
