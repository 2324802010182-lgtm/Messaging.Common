using Messaging.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Các dịch vụ mặc định
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. ĐĂNG KÝ RABBITMQ Ở ĐÂY (Phải nằm trước builder.Build)
var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq");
builder.Services.AddRabbitMq(
    rabbitMqSettings["HostName"]!,
    rabbitMqSettings["UserName"]!,
    rabbitMqSettings["Password"]!,
    rabbitMqSettings["VirtualHost"]!
);

// 3. SAU ĐÓ MỚI ĐẾN DÒNG NÀY
var app = builder.Build();

// 4. Các cấu hình Middleware (Swagger, Routing...) nằm ở dưới này
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();