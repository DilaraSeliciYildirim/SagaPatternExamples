using O.Core.Interfaces;

namespace O.Core.Events
{
    public class StockRollbackEvent : IStockRollbackEvent
    {
        public List<OrderItemMessage> OrderItems { get ; set; }
    }
}
