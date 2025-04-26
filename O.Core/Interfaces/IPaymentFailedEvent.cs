using MassTransit;

namespace O.Core.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
        public List<OrderItemMessage> orderItems { get; set; }
    }
}
