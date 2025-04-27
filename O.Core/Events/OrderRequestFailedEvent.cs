using O.Core.Interfaces;

namespace O.Core.Events
{
    public class OrderRequestFailedEvent : IOrderRequestFailedEvent
    {
        public int OrderId { get ; set; }
        public string Reason { get; set; }
    }
}
