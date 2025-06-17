using System;
namespace SmartHostelManagementSystem.Exceptions
{
    public class RoomAlreadyAssignedException : Exception
    {
        public RoomAlreadyAssignedException(string message) : base(message) { }
    }
}
