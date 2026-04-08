using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace UrbanBites.Web.Services
{
    public class NotificationService
    {
        private readonly SendGridClient _sendGridClient;

        public NotificationService()
        {
            _sendGridClient = new SendGridClient("SG.fake_api_key_for_demo");
        }

        public async Task SendOrderConfirmation(string email, int orderId)
        {
            var message = new SendGridMessage
            {
                From = new EmailAddress("noreply@urbanbites.com", "UrbanBites"),
                Subject = $"Order Confirmation #{orderId}",
                PlainTextContent = $"Your order #{orderId} has been placed successfully!"
            };
            message.AddTo(email);

            await _sendGridClient.SendEmailAsync(message);
        }

        public async Task SendOrderStatusUpdate(string email, int orderId, string status)
        {
            var message = new SendGridMessage
            {
                From = new EmailAddress("noreply@urbanbites.com", "UrbanBites"),
                Subject = $"Order Update #{orderId}",
                PlainTextContent = $"Your order #{orderId} status: {status}"
            };
            message.AddTo(email);

            await _sendGridClient.SendEmailAsync(message);
        }
    }
}
