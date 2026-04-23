using ProjectMaui.Domain.Models;
using ProjectMaui.Domain.Services;

namespace ProjectMaui.Domain.Services
{
    public class DataSeedService
    {
        private readonly UserServices _userSvc;
        private readonly ProductServices _productSvc;
        private readonly OrderService _orderSvc;

        public DataSeedService(UserServices userSvc, ProductServices productSvc, OrderService orderSvc)
        {
            _userSvc = userSvc;
            _productSvc = productSvc;
            _orderSvc = orderSvc;
        }

        public async Task SeedAllData()
        {
            var existingUsers = await _userSvc.GetAllEmployees();
            if (existingUsers.Any()) return;

            var admin = new Employee("admin", "admin123", "Muhammad Revaliza", UserRole.Admin.ToString(), "EMP001", DateTime.Now);
            await _userSvc.AddUser(admin, User.UserTypes.Employee);

            var customer = new Customer("reva_cust", "cust123", "Revaliza Customer", UserRole.Customer.ToString(), "reva@email.com");
            await _userSvc.AddUser(customer, User.UserTypes.Customer);

            var food = new Food("Nasi Goreng Spesial", "Nasi goreng dengan telur mata sapi", 25000, "nasi_goreng.jpg", "Nasi, Telur, Kecap", 50, TimeSpan.FromMinutes(10), ProductStatus.Available, "Gurih", "450 kcal");
            await _productSvc.AddProduct(food, Product.ProductTypes.Food, null);

            var drink = new Drink("Es Teh Manis", "Teh melati segar", 5000, "es_teh.jpg", "Teh, Gula, Es", 100, TimeSpan.FromMinutes(3), ProductStatus.Available, SugarLevel.Normal, false);
            await _productSvc.AddProduct(drink, Product.ProductTypes.Drink, null);

            var dessert = new Dessert("Mochi Ice Cream", "Mochi kenyal isi es krim", 15000, "mochi.jpg", "Tepung Ketan, Susu", 20, TimeSpan.FromMinutes(2), ProductStatus.Available, "Manis", "200 kcal", 5, ServingTemp.Cold);
            await _productSvc.AddProduct(dessert, Product.ProductTypes.Dessert, null);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Placed
            };

            var orderItem = new OrderItem(2, 25000, ItemStatus.Pending)
            {
                OrderId = order.Id,
                ProductId = food.Id
            };

            order.OrderItems.Add(orderItem);
            await _orderSvc.AddOrderAsync(order);
        }
    }
}