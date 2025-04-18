using Core.Events;
using MassTransit;
using Order.API.Models;

namespace Order.API.Consumers
{
    public class PaymentSuccededEventConsumer : IConsumer<PaymentSucceededEvent>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<PaymentSuccededEventConsumer> _logger;

        public PaymentSuccededEventConsumer(AppDbContext appDbContext, ILogger<PaymentSuccededEventConsumer> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
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
