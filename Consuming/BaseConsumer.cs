using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Messaging.Common.Consuming
{
    public abstract class BaseConsumer<T> : IBaseConsumer
    {
        protected readonly IModel _channel;

        public BaseConsumer(IModel channel)
        {
            _channel = channel;
        }

        public virtual async Task ConsumeAsync(string queueName)
        {
            // Khai báo Queue (đảm bảo queue tồn tại trước khi nhận)
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            // Sự kiện xảy ra khi có tin nhắn mới tới
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                // Giải mã JSON thành đối tượng T
                var message = JsonSerializer.Deserialize<T>(messageJson);

                if (message != null)
                {
                    // Gọi hàm xử lý logic (sẽ được viết ở lớp con)
                    await ProcessMessageAsync(message, ea.BasicProperties.CorrelationId);
                }

                // Xác nhận với RabbitMQ là đã nhận tin thành công (Acknowledge)
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            // Bắt đầu lắng nghe
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            await Task.CompletedTask;
        }

        // Lớp con (ví dụ PaymentConsumer) sẽ phải ghi đè hàm này để xử lý nghiệp vụ riêng
        protected abstract Task ProcessMessageAsync(T message, string correlationId);
    }
}