using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrbanBites.Web.Services;

namespace UrbanBites.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{orderId}/process")]
        public async Task<ActionResult> ProcessPayment(int orderId, [FromBody] PaymentRequest request)
        {
            try
            {
                await _paymentService.ProcessPayment(orderId, request.PaymentMethodId);
                return Ok(new { success = true });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class PaymentRequest
    {
        public string PaymentMethodId { get; set; }
    }
}
