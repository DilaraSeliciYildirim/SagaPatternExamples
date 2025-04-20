using Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddMassTransit(x => {

    x.AddConsumer<PaymentSuccededEventConsumer>();
    x.AddConsumer<PaymentFailedEventConsumer>();
    x.UsingRabbitMq((context, cfg) => {

        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        // bence burda queue tanımlamaya gerek yok. Çünkü Payment'dan event publish edildi. Özellikle bi queue'ya atılmadı.7
        cfg.ReceiveEndpoint(e => { // evet queue tanımlamadım sadece aşağıdaki ayarla çalıştı

            e.ConfigureConsumer<PaymentSuccededEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(e => { 

            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });
    });
   
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();


