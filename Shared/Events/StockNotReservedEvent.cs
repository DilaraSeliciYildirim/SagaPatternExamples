﻿namespace Core.Events
{
    public class StockNotReservedEvent
    {
        public int OrderId { get; set; }
        public string Message { get; set; }
    }
}
