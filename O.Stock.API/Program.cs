using MassTransit;
using Microsoft.EntityFrameworkCore;
using O.Core;
using O.Stock.API.Consumers;
using O.Stock.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("StockDb");
});

builder.Services.AddMassTransit(x => {

    x.AddConsumer<OrderCreatedEventConsumer>();
    x.UsingRabbitMq((context, cfg) => {

        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        // queue name vermeden de bu şekilde çalışıyor çünkü send kullanmadık
        cfg.ReceiveEndpoint(e => {

            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Stocks.Add(new Stock { Id = 1, ProductId = 1, Count = 100 });
    context.Stocks.Add(new Stock { Id = 2, ProductId = 2, Count = 100 });
    context.SaveChanges();
}

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
