using O.Core.Interfaces;

namespace O.Core.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public StockReservedEvent(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
        public List<OrderItemMessage> OrderItems { get; set ; }

        public Guid CorrelationId { get; set; }
    }
}
