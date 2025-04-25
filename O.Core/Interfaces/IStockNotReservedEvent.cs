using MassTransit;

namespace O.Core.Interfaces
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
    }
}
