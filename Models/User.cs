using ProjectMaui.Common;
using SQLite;

namespace ProjectMaui.Models
{
    public class User
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; private set; } = default!;
        public string Password { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public string Role { get; private set; } = default!;

        public User() { }

        public User(string username, string password, string fullName, string role)
        {
            Username = Guard.NotNullOrWhiteSpace(username, nameof(username));
            Password = Guard.NotNullOrWhiteSpace(password, nameof(password));
            FullName = Guard.NotNullOrWhiteSpace(fullName, nameof(fullName));
            Role = role;
        }

        public void UpdateProfile(string fullName)
        {
            FullName = Guard.NotNullOrWhiteSpace(fullName, nameof(fullName));
            Console.WriteLine($"User {Username} updated name to: {fullName}");
        }
    }

    public class Employee : User
    {
        public string EmployeeId { get; private set; } = default!;
        public DateTime JoinDate { get; private set; }

        public Employee() : base() { }

        public Employee(string username, string password, string fullName, string role, string employeeId, DateTime joinDate)
            : base(username, password, fullName, role)
        {
            EmployeeId = Guard.NotNullOrWhiteSpace(employeeId, nameof(employeeId));
            JoinDate = joinDate;
        }
    }
}