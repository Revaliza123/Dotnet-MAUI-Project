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


        public async Task AddUser(User user, User.UserTypes type)
        {
            var db = await GetDb();
            int result = 0;

            switch (type)
            {
                case User.UserTypes.Employee:
                    result = await db.InsertAsync((Employee)user);
                    break;
                case User.UserTypes.Customer:
                    result = await db.InsertAsync((Customer)user);
                    break;
            }
            if (result > 0) Console.WriteLine($"Berhasil menambah {type}");
        }

        public async Task UpdateUser(User user, User.UserTypes type)
        {
            var db = await GetDb();
            int result = 0;

            switch (type)
            {
                case User.UserTypes.Employee:
                    result = await db.UpdateAsync((Employee)user);
                    break;
            }

            if (result > 0) Console.WriteLine($"Berhasil update {type}");
        }

        public async Task DeleteUser(Guid userId, User.UserTypes type)
        {
            var db = await GetDb();
            switch (type)
            {
                case User.UserTypes.Employee:
                    await db.DeleteAsync<Employee>(userId);
                    break;
                case User.UserTypes.Customer:
                    await db.DeleteAsync<Customer>(userId);
                    break;
            }
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            var db = await GetDb();

            var employee = await db.Table<Employee>().FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            return employee;
        }

        public async Task<List<User>> GetAllEmployees()
        {
            var db = await GetDb();
            var data = await db.Table<Employee>().ToListAsync();
            return data.Cast<User>().ToList();
        }
    }
}