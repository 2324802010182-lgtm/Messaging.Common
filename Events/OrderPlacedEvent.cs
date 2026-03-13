using Messaging.Common.Models;

namespace Messaging.Common.Events
{
    // Kế thừa từ EventBase để có sẵn EventId và Timestamp
    public class OrderPlacedEvent : EventBase
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}