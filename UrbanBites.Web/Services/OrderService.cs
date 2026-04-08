using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrbanBites.Web.Controllers;
using UrbanBites.Web.Data;
using UrbanBites.Web.Models;

namespace UrbanBites.Web.Services
{
    public class OrderService
    {
        private readonly UrbanBitesDbContext _context;
        private readonly PaymentService _paymentService;
        private readonly NotificationService _notificationService;
        private readonly PricingService _pricingService;

        public OrderService(
            UrbanBitesDbContext context,
            PaymentService paymentService,
            NotificationService notificationService,
            PricingService pricingService)
        {
            _context = context;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _pricingService = pricingService;
        }

        public async Task<Order> PlaceOrder(
            int customerId,
            int restaurantId,
            OrderItemRequest[] items,
            string deliveryAddress)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new Exception("Customer not found");

            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null || !restaurant.IsActive)
                throw new Exception("Restaurant not available");

            decimal totalAmount = 0;
            foreach (var item in items)
            {
                var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (menuItem == null || !menuItem.IsAvailable)
                    throw new Exception($"Menu item {item.MenuItemId} not available");

                totalAmount += menuItem.Price * item.Quantity;
            }

            totalAmount = _pricingService.CalculateTotal(totalAmount, restaurantId);

            var order = new Order
            {
                CustomerId = customerId,
                RestaurantId = restaurantId,
                Status = "Pending",
                TotalAmount = totalAmount,
                PlacedAt = DateTime.UtcNow,
                DeliveryAddress = deliveryAddress
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var menuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    Price = menuItem.Price,
                    SpecialInstructions = item.SpecialInstructions
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            await _notificationService.SendOrderConfirmation(customer.Email, order.Id);

            return order;
        }

        public async Task<Order> GetOrder(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();

                var customer = await _context.Customers.FindAsync(order.CustomerId);
                await _notificationService.SendOrderStatusUpdate(customer.Email, orderId, status);
            }
        }
    }
}
