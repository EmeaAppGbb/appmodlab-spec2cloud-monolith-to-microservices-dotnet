using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrbanBites.Web.Data;
using UrbanBites.Web.Models;

namespace UrbanBites.Web.Services
{
    public class DeliveryService
    {
        private readonly UrbanBitesDbContext _context;

        public DeliveryService(UrbanBitesDbContext context)
        {
            _context = context;
        }

        public async Task<Driver> AssignDriver(int orderId)
        {
            var availableDriver = await _context.Drivers
                .Where(d => d.IsAvailable)
                .OrderByDescending(d => d.Rating)
                .FirstOrDefaultAsync();

            if (availableDriver == null)
                return null;

            var delivery = new Delivery
            {
                OrderId = orderId,
                DriverId = availableDriver.Id,
                Status = "Assigned"
            };

            _context.Deliveries.Add(delivery);

            availableDriver.IsAvailable = false;
            await _context.SaveChangesAsync();

            return availableDriver;
        }

        public async Task UpdateDeliveryLocation(int deliveryId, string location)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery != null)
            {
                delivery.CurrentLocation = location;
                await _context.SaveChangesAsync();
            }
        }
    }
}
