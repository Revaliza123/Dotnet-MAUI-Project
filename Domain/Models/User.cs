using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public abstract class User
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Role { get; set; } = default!;

        public enum UserTypes { Employee, Customer }

        [Ignore]
        public abstract UserTypes Type { get; }

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

    [Table("Employees")]
    public class Employee : User
    {
        public string EmployeeId { get; private set; } = default!;
        public DateTime JoinDate { get; private set; }

        [Ignore]
        public override UserTypes Type => UserTypes.Employee;

        public Employee() : base() { }

        public Employee(string username, string password, string fullName, string role, string employeeId, DateTime joinDate)
            : base(username, password, fullName, role)
        {
            EmployeeId = Guard.NotNullOrWhiteSpace(employeeId, nameof(employeeId));
            JoinDate = joinDate;
        }
    }

    [Table("Customers")]
    public class Customer : User
    {
        public int LoyaltyPoints { get; set; }
        public string Email { get; set; } = default!;

        [Ignore]
        public override UserTypes Type => UserTypes.Customer;

        public Customer() : base() { }

        public Customer(string username, string password, string fullName, string role, string email)
            : base(username, password, fullName, role)
        {
            Email = email;
            LoyaltyPoints = 0;
        }
    }
}