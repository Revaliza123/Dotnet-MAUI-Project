using ProjectMaui.Domain.Infrasturcture;
using ProjectMaui.Domain.Models;
using SQLite;

namespace ProjectMaui.Domain.Services
{
    public class TableServices
    {
        private readonly DatabaseService? database;

        private SQLiteAsyncConnection connection;

        public TableServices(DatabaseService db)
        {
            database = db;
        }

        private async Task<SQLiteAsyncConnection> GetDb()
        {
            if (connection == null)
            {
                connection = await database.GetConnection();
            }
            return connection;
        }

        public async Task<List<Table>> GetTableData()
        {
            try
            {
                var dbConnect = await GetDb();
                var tables = await dbConnect.Table<Table>().ToListAsync();
                return tables;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task AddTable(Table table)
        {
            try
            {
                var db = await GetDb();
                int result = await db.InsertAsync(table);

                if (result > 0)
                {
                    Console.WriteLine($"Succes to add the new data");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when get the data");
                throw;
            }
        }

        public async Task UpdateTable(Table table)
        {
            try
            {
                var db = await GetDb();
                int result = await db.UpdateAsync(table);

                if (result > 0)
                {
                    Console.WriteLine($"Succes to update the data");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Error {exc.Message} when update the data");
                throw;
            }
        }
        public async Task UpdateTable(Guid tableId)
        {
            var db = await GetDb();
            await db.DeleteAsync(tableId);
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