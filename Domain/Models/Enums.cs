namespace ProjectMaui.Domain.Models
{
    public enum ProductStatus { Available, OutOfStock, Discontinued }
    public enum SugarLevel { Normal, Less, NoSugar }
    public enum ServingTemp { Cold, Hot, RoomTemperature }
    public enum UserRole { Admin, Customer, Cashier, Chef }
    public enum PaymentMethod { Cash, Qris, DebitCard, CreditCard }
    public enum PaymentStatus { Pending, Success, Failed, Refunded }
    public enum OrderStatus { Placed, Preparing, ReadyToServe, Completed, Canceled }
    public enum ItemStatus { Pending, Cooking, Ready, Served }
    public enum TableStatus { Available, Occupied, Reserved, Cleaning }
}