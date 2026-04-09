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
            try {
                var dbConnect = await database.GetConnection();
                var tables = await dbConnect.Table<Table>().ToListAsync();
                return tables;
            }
            catch (Exception exc) {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task AddTable(Table table)
        {
            try {
                var db = await database.GetConnection();
                int result = await db.InsertAsync(table);

                if (result > 0) {
                    Console.WriteLine($"Succes to add the new data");
                }
            }
            catch (Exception exc) {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
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