using ProjectMaui.Domain.Common;
using SQLite;

namespace ProjectMaui.Domain.Models
{
    public class User
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; private set; } = default!;
        public string Password { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public string Email { get; private set; } = default!; //
        public string PhoneNumber { get; private set; } = default!; // 
        public string Role { get; private set; } = default!;
        public DateTime DateOfBirth { get; private set; } // 

        public User() { }

                public User(string username, string password, string fullName, string email, string phoneNumber, string role, DateTime dateOfBirth)
        {
            Username = Guard.NotNullOrWhiteSpace(username, nameof(username));
            Password = Guard.NotNullOrWhiteSpace(password, nameof(password));
            FullName = Guard.NotNullOrWhiteSpace(fullName, nameof(fullName));
            Email = Guard.NotNullOrWhiteSpace(email, nameof(email));
            PhoneNumber = Guard.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Role = role;
            DateOfBirth = dateOfBirth;
        }

        public void UpdateProfile(string fullName, string email, string phoneNumber)
        {
            FullName = Guard.NotNullOrWhiteSpace(fullName, nameof(fullName));
            Email = Guard.NotNullOrWhiteSpace(email, nameof(email));
            PhoneNumber = Guard.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            
            Console.WriteLine($"User {Username} updated profile details.");
        }
    }

    public class Employee : User
    {
        public string EmployeeId { get; private set; } = default!;
        public DateTime JoinDate { get; private set; }

        public Employee() : base() { }

       
        public Employee(string username, string password, string fullName, string email, string phoneNumber, string role, DateTime dateOfBirth, string employeeId, DateTime joinDate)
            : base(username, password, fullName, email, phoneNumber, role, dateOfBirth)
        {
            EmployeeId = Guard.NotNullOrWhiteSpace(employeeId, nameof(employeeId));
            JoinDate = joinDate;
        }
    }
}