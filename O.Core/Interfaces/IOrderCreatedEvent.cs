using MassTransit;

namespace O.Core.Interfaces
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>  // MassTransit hangi event olduğunu takip edebilsin diye
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
