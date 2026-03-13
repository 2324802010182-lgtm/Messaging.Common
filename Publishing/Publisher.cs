using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Messaging.Common.Publishing
{
    public class Publisher
    {
        // Channel (IModel) là nơi thực hiện các lệnh gửi/nhận tin nhắn cụ thể
        private readonly IModel _channel;

        public Publisher(IModel channel)
        {
            _channel = channel;
        }

        // T: Là kiểu dữ liệu của tin nhắn (ví dụ: OrderPlacedEvent)
        public void Publish<T>(string exchange, string routingKey, T message, string? correlationId = null)
        {
            // 1. Chuyển đối tượng tin nhắn thành chuỗi JSON và mã hóa thành mảng byte
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            // 2. Thiết lập thuộc tính cho tin nhắn
            var props = _channel.CreateBasicProperties();
            props.Persistent = true; // Đảm bảo tin nhắn không bị mất nếu RabbitMQ server bị restart
            props.CorrelationId = correlationId ?? Guid.NewGuid().ToString();

            // 3. Gửi tin nhắn lên Exchange
            _channel.BasicPublish(
                exchange: exchange,          // Tên sàn giao dịch
                routingKey: routingKey,      // Định hướng tin nhắn đến Queue nào
                basicProperties: props,      // Các thuộc tính đính kèm
                body: body                   // Nội dung tin nhắn
            );
        }
    }
}