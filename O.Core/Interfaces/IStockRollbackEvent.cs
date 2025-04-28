namespace O.Core.Interfaces
{
    public interface IStockRollbackEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
