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

        
        public async Task<bool> Authenticate(string username, string password)
        {
            var db = await GetDb();
            // Langsung cari di tabel User
            var user = await db.Table<User>()
                               .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var db = await GetDb();
            return await db.Table<User>().ToListAsync();
        }
    }
}