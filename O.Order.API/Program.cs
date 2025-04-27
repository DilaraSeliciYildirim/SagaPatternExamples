using MassTransit;
using Microsoft.EntityFrameworkCore;
using O.Core;
using O.Core.Events;
using O.Order.API.Consumers;
using O.Order.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x => {

    x.AddConsumer<OrderRequestCompletedEventConsumer>();
    x.AddConsumer<OrderRequestFailedEventConsumer>();
    x.UsingRabbitMq((context, cfg) => {

        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        // bence burda queue tanımlamaya gerek yok. Çünkü Payment'dan event publish edildi. Özellikle bi queue'ya atılmadı.7
        cfg.ReceiveEndpoint( e => {

            e.ConfigureConsumer<OrderRequestCompletedEventConsumer>(context);
            e.ConfigureConsumer<OrderRequestFailedEventConsumer>(context);
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

app.UseAuthorization();

app.MapControllers();

app.Run();
