namespace Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public DateTime EmployeeSince { get; set; }

    public Team Team { get; set; } = new();
    public IEnumerable<Specialization> Specializations { get; set; } = Array.Empty<Specialization>();
}