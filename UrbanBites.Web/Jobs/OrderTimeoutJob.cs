using System;
using System.Linq;
using UrbanBites.Web.Data;

namespace UrbanBites.Web.Jobs
{
    public class OrderTimeoutJob
    {
        private readonly UrbanBitesDbContext _context;

        public OrderTimeoutJob(UrbanBitesDbContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            var timeoutThreshold = DateTime.UtcNow.AddMinutes(-30);
            var timedOutOrders = _context.Orders
                .Where(o => o.Status == "Pending" && o.PlacedAt < timeoutThreshold)
                .ToList();

            foreach (var order in timedOutOrders)
            {
                order.Status = "Cancelled";
            }

            _context.SaveChanges();
        }
    }
}
