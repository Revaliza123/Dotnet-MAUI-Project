// DataSeedService.cs
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

            var nasiGoreng = new Food(
                "Nasi Goreng Spesial",
                "Nasi goreng dengan telur mata sapi, ayam, dan kerupuk",
                25000,
                "nasi_goreng_spesial.jpg",
                "Nasi, Telur, Ayam, Kecap, Kerupuk",
                50, TimeSpan.FromMinutes(10),
                ProductStatus.Available, "Gurih", "450 kcal");
            await _productSvc.AddProduct(nasiGoreng, Product.ProductTypes.Food, null);

            var ayamBakar = new Food(
                "Ayam Bakar Madu",
                "Ayam kampung bakar dengan bumbu madu pilihan",
                30000,
                "ayam_bakar_madu.jpg",
                "Ayam Kampung, Madu, Kecap, Bumbu Rempah",
                30, TimeSpan.FromMinutes(20),
                ProductStatus.Available, "Manis Gurih", "520 kcal");
            await _productSvc.AddProduct(ayamBakar, Product.ProductTypes.Food, null);

            var esTeh = new Drink(
                "Es Teh Manis",
                "Teh melati segar dengan es batu pilihan",
                5000,
                "drinks/es_teh_manis.jpg",
                "Teh Melati, Gula, Es Batu",
                100, TimeSpan.FromMinutes(3),
                ProductStatus.Available, SugarLevel.Normal, false);
            await _productSvc.AddProduct(esTeh, Product.ProductTypes.Drink, null);

            var kopiSusu = new Drink(
                "Kopi Susu Aren",
                "Espresso dengan gula aren dan susu segar",
                18000,
                "kopi_susu_aren.jpg",
                "Espresso, Gula Aren, Susu Segar, Es Batu",
                50, TimeSpan.FromMinutes(5),
                ProductStatus.Available, SugarLevel.Normal, true);
            await _productSvc.AddProduct(kopiSusu, Product.ProductTypes.Drink, null);

            var mochi = new Dessert(
                "Mochi Ice Cream",
                "Mochi kenyal dengan isian es krim berbagai rasa",
                15000,
                "mochi_ice_cream.jpg",
                "Tepung Ketan, Susu, Gula, Es Krim",
                20, TimeSpan.FromMinutes(2),
                ProductStatus.Available, "Manis", "200 kcal", 5, ServingTemp.Cold);
            await _productSvc.AddProduct(mochi, Product.ProductTypes.Dessert, null);

            var esKrimStrawberry = new Dessert(
                "Es Krim Strawberry",
                "Es krim lembut dengan topping strawberry segar",
                12000,
                "es_krim_strawberry.jpg",
                "Susu, Gula, Strawberry, Krim",
                25, TimeSpan.FromMinutes(2),
                ProductStatus.Available, "Manis Segar", "180 kcal", 4, ServingTemp.Cold);
            await _productSvc.AddProduct(esKrimStrawberry, Product.ProductTypes.Dessert, null);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Placed
            };

            var orderItem = new OrderItem(2, 25000, ItemStatus.Pending)
            {
                OrderId = order.Id,
                ProductId = nasiGoreng.Id
            };

            order.OrderItems.Add(orderItem);
            await _orderSvc.AddOrderAsync(order);
        }
    }
}