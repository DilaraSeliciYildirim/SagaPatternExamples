using O.Core.Interfaces;

namespace O.Core.Events
{
    public class PaymentCompletedEvent : IPaymentCompletedEvent
    {
        public PaymentCompletedEvent(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
        public int OrderId { get; set ; }

        public Guid CorrelationId { get; set; }
    }
}
