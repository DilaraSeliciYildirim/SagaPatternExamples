using MassTransit;

namespace O.Core.Interfaces
{
    public interface IStockReservedRequestPaymentEvent : CorrelatedBy<Guid>
    {
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } // ödeme basarisiz olursa elimizde compansate transaction için olmalı
    }
}
