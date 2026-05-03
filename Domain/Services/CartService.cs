using ProjectMaui.Domain.DTOs;
using ProjectMaui.Domain.Models;

namespace ProjectMaui.Domain.Services;

public class CartService
{
    private readonly List<CartItem> _cartItems = new();

    public event Action? OnCartChanged;

    public List<CartItem> Items => new(_cartItems);

    public bool AddItem(Product product)
    {
        var existing = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            _cartItems.Add(new CartItem(product));
        }
        OnCartChanged?.Invoke();
        return existing != null;
    }

    public void IncreaseQuantity(Guid productId)
    {
        var existing = _cartItems.FirstOrDefault(item => item.Product.Id == productId);
        if (existing != null) existing.Quantity++;
        OnCartChanged?.Invoke();
    }

    public void DecreaseQuantity(Guid productId)
    {
        var existing = _cartItems.FirstOrDefault(item => item.Product.Id == productId);
        if (existing != null)
        {
            existing.Quantity--;
            if (existing.Quantity <= 0)
            {
                _cartItems.Remove(existing);
            }
        }
        OnCartChanged?.Invoke();
    }

    public void RemoveItem(Guid productId)
    {
        _cartItems.RemoveAll(item => item.Product.Id == productId);
        OnCartChanged?.Invoke();
    }

    public void Clear()
    {
        _cartItems.Clear();
        OnCartChanged?.Invoke();
    }

    public int TotalItems => _cartItems.Count;

    public int TotalQuantity => _cartItems.Sum(item => item.Quantity);

    public decimal TotalPrice => _cartItems.Sum(item => item.SubTotal);

    public bool IsEmpty => _cartItems.Count == 0;
}
