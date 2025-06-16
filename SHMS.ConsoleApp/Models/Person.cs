namespace SmartHostelManagementSystem.Models;

public class Person
{
    public int ID { get; set; }
    public string Name { get; set; }

    public Person() { }

    public Person(int id, string name)
    {
        ID = id;
        Name = name;
    }
}
