using ProjectMaui.Models;

namespace DotnetMauiProject.Services
{
    public class TableServices
    {
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