using ProjectMaui.Models;
using ProjectMaui.Services;

namespace DotnetMauiProject.Services
{
    public class TableServices
    {
        private readonly DatabaseService? database;

        public TableServices(DatabaseService db)
        {
            database = db;
        }

        public async Task<List<Table>> GetTableData()
        {
            var dbConnect = await database.GetConnection();
            var tables = await dbConnect.Table<Table>().ToListAsync();
            return tables;
        }
        public void UpdateTableStatus(int tableId, TableStatus tableStatus)
        {
            Console.WriteLine($"{tableId} orders");
            Console.WriteLine($"{tableStatus} status");
        }
        public void AssignOrder(int tableId)
        {
            Console.WriteLine($"{tableId} orders");
        }
        public void ReleaseTable(int tableId)
        {
            Console.WriteLine($"{tableId} orders");
        }
    }
}