using System.Linq;
using UrbanBites.Web.Data;

namespace UrbanBites.Web.Services
{
    public class ReviewService
    {
        private readonly UrbanBitesDbContext _context;

        public ReviewService(UrbanBitesDbContext context)
        {
            _context = context;
        }

        public void ModerateReview(int reviewId, bool approved)
        {
            var review = _context.Reviews.Find(reviewId);
            if (review != null)
            {
                review.IsModerated = approved;
                _context.SaveChanges();
            }
        }

        public decimal CalculateRestaurantRating(int restaurantId)
        {
            var reviews = _context.Reviews
                .Where(r => r.RestaurantId == restaurantId && r.IsModerated)
                .ToList();

            if (!reviews.Any())
                return 0;

            return (decimal)reviews.Average(r => r.Rating);
        }
    }
}
