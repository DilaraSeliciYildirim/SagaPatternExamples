﻿namespace Core.Events
{
    public class PaymentFailedEvent
    {
        public int OrderId { get; set; }
        public string Message { get; set; }
    }
}
