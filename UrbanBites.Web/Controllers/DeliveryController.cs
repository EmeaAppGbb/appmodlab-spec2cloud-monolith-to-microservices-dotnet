using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UrbanBites.Web.Data;
using UrbanBites.Web.Models;

namespace UrbanBites.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly UrbanBitesDbContext _context;

        public DeliveryController(UrbanBitesDbContext context)
        {
            _context = context;
        }

        [HttpGet("{orderId}")]
        public ActionResult<Delivery> GetDelivery(int orderId)
        {
            var delivery = _context.Deliveries.FirstOrDefault(d => d.OrderId == orderId);
            if (delivery == null)
                return NotFound();

            return Ok(delivery);
        }

        [HttpGet("drivers/available")]
        public ActionResult<IEnumerable<Driver>> GetAvailableDrivers()
        {
            var drivers = _context.Drivers.Where(d => d.IsAvailable).ToList();
            return Ok(drivers);
        }
    }
}
