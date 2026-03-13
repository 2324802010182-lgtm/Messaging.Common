using Messaging.Common.Consuming;
using Messaging.Common.Events;
using RabbitMQ.Client;

namespace Payment.API
{
    // Kế thừa BaseConsumer đã viết ở Bước 4
    public class PaymentConsumer : BaseConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<PaymentConsumer> _logger;

        public PaymentConsumer(IModel channel, ILogger<PaymentConsumer> logger) : base(channel)
        {
            _logger = logger;
        }

        // Đây là nơi xử lý logic khi nhận được tin nhắn
        protected override Task ProcessMessageAsync(OrderPlacedEvent message, string correlationId)
        {
            // Giả lập xử lý thanh toán
            _logger.LogInformation("--- ĐÃ NHẬN ĐƠN HÀNG ---");
            _logger.LogInformation("Mã đơn: {OrderId}", message.OrderId);
            _logger.LogInformation("Khách hàng: {CustomerName}", message.CustomerName);
            _logger.LogInformation("Tổng tiền: {TotalAmount}", message.TotalAmount);
            _logger.LogInformation("CorrelationId: {CorrelationId}", correlationId);
            _logger.LogInformation("-------------------------");

            return Task.CompletedTask;
        }
    }
}