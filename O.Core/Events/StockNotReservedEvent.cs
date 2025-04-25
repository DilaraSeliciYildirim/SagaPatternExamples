using O.Core.Interfaces;

namespace O.Core.Events
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {
        public StockNotReservedEvent(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
        public string Reason { get; set; }

        public Guid CorrelationId {get; set;}
    }
}
