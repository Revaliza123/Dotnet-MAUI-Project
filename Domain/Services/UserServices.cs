using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using SQLite;

namespace ProjectMaui.Domain.Services;

public class UserServices
{
    private readonly DatabaseService _databaseService;
    private SQLiteAsyncConnection? _connection;

    public UserServices(DatabaseService databaseService)
    {
        _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
    }

    private async Task<SQLiteAsyncConnection> GetDb()
    {
        if (_connection == null)
        {
            _connection ??= await _databaseService.GetConnection();
        }
        return _connection;
    }

    public async Task<User> AddUser(User user, User.UserTypes userType)
    {
        var db = await GetDb();
        if (userType == User.UserTypes.Employee)
            await db.InsertAsync((Employee)user);
        else
            await db.InsertAsync((Customer)user);
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        var db = await GetDb();
        if (user is Employee emp)
            await db.UpdateAsync(emp);
        else if (user is Customer cust)
            await db.UpdateAsync(cust);
        return user;
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var db = await GetDb();

        var employee = await db.Table<Employee>().FirstOrDefaultAsync(u => u.Id == id);
        if (employee != null)
        {
            await db.DeleteAsync(employee);
            return true;
        }

        var customer = await db.Table<Customer>().FirstOrDefaultAsync(u => u.Id == id);
        if (customer != null)
        {
            await db.DeleteAsync(customer);
            return true;
        }
        return false;
    }

    public async Task<User?> Authenticate(string username, string password)
    {
        var db = await GetDb();

        var employee = await db.Table<Employee>()
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        if (employee != null) return employee;

        var customer = await db.Table<Customer>()
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        return customer;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var db = await GetDb();
        return await db.Table<Employee>().ToListAsync();
    }

    public async Task<List<Customer>> GetAllCustomers()
    {
        var db = await GetDb();
        return await db.Table<Customer>().ToListAsync();
    }
}