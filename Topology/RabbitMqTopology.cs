using RabbitMQ.Client;

namespace Messaging.Common.Topology
{
    public class RabbitMqTopology
    {
        private readonly IModel _channel;

        public RabbitMqTopology(IModel channel)
        {
            _channel = channel;
        }

        // Phương thức này sẽ khai báo các thành phần cần thiết
        public void DeclareOrderTopology()
        {
            // 1. Khai báo Exchange (Sàn giao dịch)
            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Topic, durable: true);

            // 2. Khai báo Queue (Hàng đợi)
            _channel.QueueDeclare(queue: "order_placed_queue", durable: true, exclusive: false, autoDelete: false);

            // 3. Bind (Gắn) Queue vào Exchange với một Routing Key cụ thể
            _channel.QueueBind(queue: "order_placed_queue", exchange: "order_exchange", routingKey: "order.placed");
        }
    }
}