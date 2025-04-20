using Core.Events;
using MassTransit;
using Order.API.Models;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<PaymentSuccededEventConsumer> _logger;

        public PaymentFailedEventConsumer(AppDbContext appDbContext, ILogger<PaymentSuccededEventConsumer> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _appDbContext.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
                await _appDbContext.SaveChangesAsync();

                _logger.LogInformation($"Order Id {order.Id} status changed: {order.Status}");
            }
        }
    }
}
