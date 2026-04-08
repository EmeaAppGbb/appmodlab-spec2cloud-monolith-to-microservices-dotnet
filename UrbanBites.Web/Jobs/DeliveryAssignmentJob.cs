using System.Linq;
using UrbanBites.Web.Data;
using UrbanBites.Web.Services;

namespace UrbanBites.Web.Jobs
{
    public class DeliveryAssignmentJob
    {
        private readonly UrbanBitesDbContext _context;
        private readonly DeliveryService _deliveryService;

        public DeliveryAssignmentJob(UrbanBitesDbContext context, DeliveryService deliveryService)
        {
            _context = context;
            _deliveryService = deliveryService;
        }

        public void Execute()
        {
            var paidOrders = _context.Orders
                .Where(o => o.Status == "Paid")
                .ToList();

            foreach (var order in paidOrders)
            {
                var existingDelivery = _context.Deliveries.FirstOrDefault(d => d.OrderId == order.Id);
                if (existingDelivery == null)
                {
                    var driver = _deliveryService.AssignDriver(order.Id).Result;
                    if (driver != null)
                    {
                        order.Status = "Assigned";
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
