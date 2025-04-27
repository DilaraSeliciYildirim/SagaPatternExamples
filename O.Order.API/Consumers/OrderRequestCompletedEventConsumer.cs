using MassTransit;
using O.Core.Interfaces;
using O.Order.API.Models;

namespace O.Order.API.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<OrderRequestCompletedEventConsumer> _logger;

        public OrderRequestCompletedEventConsumer(AppDbContext appDbContext, 
            ILogger<OrderRequestCompletedEventConsumer> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var order = await _appDbContext.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Success;
                await _appDbContext.SaveChangesAsync();

                _logger.LogInformation($"Order Id {order.Id} status changed: {order.Status}");
            }
        }
    }
}
