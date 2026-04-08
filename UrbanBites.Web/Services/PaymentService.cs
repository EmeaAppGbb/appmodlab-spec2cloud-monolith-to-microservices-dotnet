using System;
using System.Threading.Tasks;
using Stripe;
using UrbanBites.Web.Data;
using UrbanBites.Web.Models;

namespace UrbanBites.Web.Services
{
    public class PaymentService
    {
        private readonly UrbanBitesDbContext _context;

        public PaymentService(UrbanBitesDbContext context)
        {
            _context = context;
            StripeConfiguration.ApiKey = "sk_test_fake_key_for_demo";
        }

        public async Task ProcessPayment(int orderId, string paymentMethodId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100),
                Currency = "usd",
                PaymentMethod = paymentMethodId,
                Confirm = true,
            });

            var payment = new Payment
            {
                OrderId = orderId,
                StripePaymentIntentId = paymentIntent.Id,
                Amount = order.TotalAmount,
                Status = paymentIntent.Status,
                ProcessedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            if (paymentIntent.Status == "succeeded")
            {
                order.Status = "Paid";
                await _context.SaveChangesAsync();
            }
        }
    }
}
