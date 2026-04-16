using ProjectMaui.Domain.Models;

namespace ProjectMaui.Domain.DTOs;

public class ProductWithCategory
{
    public Product Product { get; set; }
    public string CategoryName { get; set; }
}