using MassTransit;
using O.Core.Events;
using O.Core.Interfaces;

namespace O.Payment.API.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPaymentEvent>
    {
        private readonly ILogger<StockReservedRequestPaymentConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedRequestPaymentConsumer(ILogger<StockReservedRequestPaymentConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IStockReservedRequestPaymentEvent> context)
        {
            var balance = 3000m;

            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn");

                await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                _logger.LogInformation("balance is not enough");

                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enough balance",
                    orderItems = context.Message.OrderItems,
                }
                );
            }
        }
    }
}
