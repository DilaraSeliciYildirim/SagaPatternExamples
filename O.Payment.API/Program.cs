using MassTransit;
using O.Core;
using O.Payment.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x => {

    x.AddConsumer<StockReservedRequestPaymentConsumer>();
    x.UsingRabbitMq((context, cfg) => {

        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        // state machine'de bu kuyruğa 'send' yapıldığı için adını vermek lazım
        cfg.ReceiveEndpoint(RabbitMQSettings.StockReservedRequestPaymentQueueName, e => { 

            e.ConfigureConsumer<StockReservedRequestPaymentConsumer>(context);
        });
    }); 

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
