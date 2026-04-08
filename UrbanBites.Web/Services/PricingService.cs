using System.Linq;
using UrbanBites.Web.Data;

namespace UrbanBites.Web.Services
{
    public class PricingService
    {
        private readonly UrbanBitesDbContext _context;

        public PricingService(UrbanBitesDbContext context)
        {
            _context = context;
        }

        public decimal CalculateTotal(decimal baseAmount, int restaurantId)
        {
            var restaurant = _context.Restaurants.Find(restaurantId);
            if (restaurant == null)
                return baseAmount;

            var commission = baseAmount * restaurant.CommissionRate;

            var activePromotions = _context.Promotions
                .Where(p => p.RestaurantId == restaurantId || p.RestaurantId == null)
                .Where(p => p.ValidFrom <= System.DateTime.UtcNow && p.ValidTo >= System.DateTime.UtcNow)
                .FirstOrDefault();

            if (activePromotions != null)
            {
                if (activePromotions.DiscountType == "Percentage")
                {
                    baseAmount -= baseAmount * (activePromotions.Value / 100);
                }
                else if (activePromotions.DiscountType == "Fixed")
                {
                    baseAmount -= activePromotions.Value;
                }
            }

            return baseAmount + commission;
        }
    }
}
