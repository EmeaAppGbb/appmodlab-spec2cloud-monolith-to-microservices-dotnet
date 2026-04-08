using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UrbanBites.Web.Models;
using UrbanBites.Web.Services;

namespace UrbanBites.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PlaceOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = await _orderService.PlaceOrder(
                    request.CustomerId,
                    request.RestaurantId,
                    request.Items,
                    request.DeliveryAddress);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            await _orderService.UpdateOrderStatus(id, status);
            return NoContent();
        }
    }

    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public OrderItemRequest[] Items { get; set; }
        public string DeliveryAddress { get; set; }
    }

    public class OrderItemRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string SpecialInstructions { get; set; }
    }
}
