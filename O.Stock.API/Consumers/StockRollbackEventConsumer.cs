using MassTransit;
using Microsoft.EntityFrameworkCore;
using O.Core.Events;
using O.Stock.API.Models;

namespace O.Stock.API.Consumers
{
    public class StockRollbackEventConsumer : IConsumer<StockRollbackEvent>
    {
        private readonly AppDbContext _appDbContext;
        private ILogger<StockRollbackEventConsumer> _logger;

        public StockRollbackEventConsumer(AppDbContext appDbContext, ILogger<StockRollbackEventConsumer> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockRollbackEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _appDbContext.SaveChangesAsync();
                }
                _logger.LogInformation($"Stock released for Product Id {item.ProductId}");
            }
        }
    }
}
