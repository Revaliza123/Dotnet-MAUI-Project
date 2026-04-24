using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Infrasturcture; 
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class UserServices
    {
        private readonly DatabaseService _databaseServices;
        private SQLiteAsyncConnection? _connection;

        public UserServices(DatabaseService database)
        {
            _databaseServices = database;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (_connection == null) 
                _connection = await _databaseServices.GetConnection();
            return _connection;
        }

        

        public async Task AddUser(User user)
        {
            var db = await GetDb();
            await db.InsertAsync(user);
        }

        public async Task UpdateUser(User user)
        {
            var db = await GetDb();
            await db.UpdateAsync(user);
        }

        public async Task DeleteUser(User user)
        {
            var db = await GetDb();
            await db.DeleteAsync(user);
        }


        public async Task AddEmployee(Employee employee)
        {
            var db = await GetDb();
            await db.InsertAsync(employee);
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var db = await GetDb();
            return await db.Table<Employee>().ToListAsync();
        }

        public async Task UpdateProfile(Employee employee, string newFullName, string newEmail, string newPhone)
        {
            var db = await GetDb();
            
            employee.UpdateProfile(newFullName, newEmail, newPhone);
            
            await db.UpdateAsync(employee);
        }


        public async Task<bool> Authenticate(string username, string password)
        {
            var db = await GetDb();
            // UserServices sekarang juga bisa mencari di tabel Employee karena Employee adalah User
            var user = await db.Table<User>()
                               .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            
            if (user == null)
            {
                var emp = await db.Table<Employee>()
                                  .FirstOrDefaultAsync(e => e.Username == username && e.Password == password);
                return emp != null;
            }

            return user != null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var db = await GetDb();
            return await db.Table<User>().ToListAsync();
        }
    }
}