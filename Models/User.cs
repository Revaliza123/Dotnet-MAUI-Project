namespace ProjectMaui.Models;

public class User 
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; 
}

// Inheritance Level 2 untuk syarat Pak Kusno
public class Employee : User 
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
}