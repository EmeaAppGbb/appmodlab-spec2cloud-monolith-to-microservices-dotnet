using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanBites.Web.Data;
using UrbanBites.Web.Models;

namespace UrbanBites.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly UrbanBitesDbContext _context;

        public RestaurantController(UrbanBitesDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            return Ok(_context.Restaurants.Where(r => r.IsActive).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpGet("{id}/menu")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenu(int id)
        {
            var menuItems = _context.MenuItems
                .Where(m => m.RestaurantId == id && m.IsAvailable)
                .ToList();

            return Ok(menuItems);
        }

        [HttpPost]
        public async Task<ActionResult<Restaurant>> CreateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, restaurant);
        }
    }
}
