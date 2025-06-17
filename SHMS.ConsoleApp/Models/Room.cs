using System;
using System.Collections.Generic;
namespace SmartHostelManagementSystem.Models;
using Models;
using System;
using System.Collections.Generic;

public class Room
{
	public int RoomNumber { get; set; }
	public int Capacity { get; set; }
	public List<Student> Occupants { get; set; }

	public Room()
	{
		Occupants = new List<Student>();
	}

	public Room(int roomNumber, int capacity)
	{
		RoomNumber = roomNumber;
		Capacity = capacity;
		Occupants = new List<Student>();
	}

	public bool IsFull() => Occupants.Count >= Capacity;

	public void AddStudent(Student student)
	{
		if (IsFull())
			throw new InvalidOperationException("Room is already full.");
		Occupants.Add(student);
	}
}
