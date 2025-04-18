using Core.Events;
using MassTransit;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn");

                await _publishEndpoint.Publish(new PaymentSucceededEvent { OrderId = context.Message.OrderId });
            }
            else
            {
                _logger.LogInformation("balance is not enough");

                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    OrderId = context.Message.OrderId,
                    Message = "balance is not enough"
                });
            }
        }
    }
}
