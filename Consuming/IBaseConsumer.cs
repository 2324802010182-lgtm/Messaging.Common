namespace Messaging.Common.Consuming
{
    public interface IBaseConsumer
    {
        // Phương thức này sẽ bắt đầu quá trình lắng nghe tin nhắn từ một Queue
        Task ConsumeAsync(string queueName);
    }
}