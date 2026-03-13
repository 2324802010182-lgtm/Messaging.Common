using Messaging.Common.Events;
using Messaging.Common.Publishing;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly Publisher _publisher;

        // Dependency Injection sẽ tự động đưa lớp Publisher vào đây
        public OrderController(Publisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderPlacedEvent orderEvent)
        {
            // 1. Thực hiện logic lưu database (bỏ qua ở bài tập này)

            // 2. Gửi tin nhắn lên RabbitMQ
            // "order_exchange": Tên sàn đã tạo ở Topology
            // "order.placed": Routing Key để dẫn đường cho tin nhắn
            _publisher.Publish("order_exchange", "order.placed", orderEvent);

            return Ok(new { Message = "Đơn hàng đã được gửi lên RabbitMQ!", OrderId = orderEvent.OrderId });
        }
    }
}