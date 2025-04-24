using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using O.Core;
using SagaStateMachine;
using SagaStateMachine.Models;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<OrderStateDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), m =>
    {
        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
    });
});

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().
    EntityFrameworkRepository(opt =>
    {
        opt.ConcurrencyMode = ConcurrencyMode.Pessimistic;
        opt.ExistingDbContext<OrderStateDbContext>();

        cfg.UsingRabbitMq((context, busCfg) =>
            {
                busCfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
                busCfg.ReceiveEndpoint(RabbitMQSettings.OrderSaga, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(context);
                });
            });
       
    });
});

var host = builder.Build();
host.Run();
