using MassTransit;

namespace O.Core.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
        public int OrderId { get; set; }
    }
}
