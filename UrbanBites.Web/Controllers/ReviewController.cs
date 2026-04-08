using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrbanBites.Web.Data;
using UrbanBites.Web.Models;

namespace UrbanBites.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly UrbanBitesDbContext _context;

        public ReviewController(UrbanBitesDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview([FromBody] Review review)
        {
            review.IsModerated = false;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }
    }
}
