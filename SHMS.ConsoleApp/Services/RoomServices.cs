
namespace SmartHostelManagementSystem.Models
{
	public class RoomService
	{
		private readonly List<Room> _rooms;

		public RoomService(List<Room> rooms)
		{
			_rooms = rooms;
		}

		public List<Student> GetAllStudents()
		{
			return _rooms.SelectMany(r => r.Occupants).ToList();
		}

	}
}