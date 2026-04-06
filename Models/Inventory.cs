namespace ProjectMaui.Models;

public class Inventory 
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public DateTime LastUpdated { get; set; }

    public bool CheckAvailability() 
    {
        return CurrentStock > MinimumStock;
    }
}