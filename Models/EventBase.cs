namespace Messaging.Common.Models
{
    public abstract class EventBase
    {
        // ID duy nhất cho mỗi tin nhắn
        public Guid EventId { get; private set; } = Guid.NewGuid();

        // Thời điểm tin nhắn được tạo ra
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        // Dùng để truy vết tin nhắn đi qua nhiều Service khác nhau
        public string? CorrelationId { get; set; }
    }
}