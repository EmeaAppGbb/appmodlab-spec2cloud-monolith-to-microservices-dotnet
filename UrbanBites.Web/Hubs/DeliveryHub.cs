using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace UrbanBites.Web.Hubs
{
    public class DeliveryHub : Hub
    {
        public async Task UpdateLocation(int deliveryId, string location)
        {
            await Clients.All.SendAsync("LocationUpdated", deliveryId, location);
        }

        public async Task JoinDeliveryTracking(int orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
        }
    }
}
