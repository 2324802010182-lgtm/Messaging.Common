using Messaging.Common.Connection;
using Messaging.Common.Topology;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Messaging.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Extension method giúp đăng ký RabbitMQ vào DI Container
        public static IServiceCollection AddRabbitMq(
            this IServiceCollection services,
            string hostName,
            string userName,
            string password,
            string vhost)
        {
            // 1. Khởi tạo ConnectionManager để quản lý kết nối
            var connectionManager = new ConnectionManager(hostName, userName, password, vhost);

            // 2. Lấy đối tượng kết nối (IConnection)
            var connection = connectionManager.GetConnection();

            // 3. Tạo một Channel (IModel) dùng chung
            var channel = connection.CreateModel();

            // 4. Đăng ký các đối tượng này dưới dạng Singleton (tồn tại suốt vòng đời ứng dụng)
            services.AddSingleton(connectionManager);
            services.AddSingleton(connection);
            services.AddSingleton(channel);

            // Khởi tạo và chạy Topology ngay khi ứng dụng start
            var topology = new RabbitMqTopology(channel);
            topology.DeclareOrderTopology();

            // Đăng ký cả Topology và Publisher vào DI để dùng ở các Service khác
            services.AddSingleton(topology);
            services.AddSingleton<Publishing.Publisher>();

            return services;
        }
    }
}