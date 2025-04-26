using O.Core.Interfaces;

namespace O.Core.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public PaymentFailedEvent(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }

        public int OrderId { get; set; }

        public Guid CorrelationId { get; set; }
        public string Reason { get; set; }
        public List<OrderItemMessage> orderItems { get; set; }
    }
}
