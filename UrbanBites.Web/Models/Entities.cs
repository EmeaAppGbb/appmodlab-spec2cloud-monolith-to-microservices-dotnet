using System;
using System.ComponentModel.DataAnnotations;

namespace UrbanBites.Web.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Address { get; set; }
        public string Cuisine { get; set; }
        public decimal Rating { get; set; }
        public bool IsActive { get; set; }
        public int OwnerId { get; set; }
        public decimal CommissionRate { get; set; }
    }

    public class MenuItem
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public int PrepTime { get; set; }
        
        public Restaurant Restaurant { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PlacedAt { get; set; }
        public string DeliveryAddress { get; set; }
        
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SpecialInstructions { get; set; }
        
        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string StripePaymentIntentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
        
        public Order Order { get; set; }
    }

    public class Delivery
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? DriverId { get; set; }
        public string Status { get; set; }
        public DateTime? PickupTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string CurrentLocation { get; set; }
        
        public Order Order { get; set; }
        public Driver Driver { get; set; }
    }

    public class Driver
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string VehicleType { get; set; }
        public bool IsAvailable { get; set; }
        public string CurrentLocation { get; set; }
        public decimal Rating { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Phone { get; set; }
        public string DefaultAddress { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public class Review
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsModerated { get; set; }
        
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public class Promotion
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DiscountType { get; set; }
        public decimal Value { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int? RestaurantId { get; set; }
        
        public Restaurant Restaurant { get; set; }
    }
}
