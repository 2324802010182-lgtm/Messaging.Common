using Messaging.Common.Extensions;
using Payment.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 1. Đăng ký RabbitMq (Lưu ý đặt TRƯỚC builder.Build)
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq");
builder.Services.AddRabbitMq(
    rabbitMqSettings["HostName"]!,
    rabbitMqSettings["UserName"]!,
    rabbitMqSettings["Password"]!,
    rabbitMqSettings["VirtualHost"]!
);

// 2. Đăng ký PaymentConsumer
builder.Services.AddSingleton<PaymentConsumer>();

var app = builder.Build();

// 3. Kích hoạt Consumer ngay khi app vừa chạy xong
var consumer = app.Services.GetRequiredService<PaymentConsumer>();
// "order_placed_queue": Tên queue bạn đã đặt ở Topology (Bước 6)
await consumer.ConsumeAsync("order_placed_queue");

app.Run();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
