﻿namespace Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public Address Address { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public  OrderStatus Status { get; set; }
        public string FailMessage { get; set; }
    }

    public enum OrderStatus
    {
        Suspend = 0,
        Success = 1,
        Fail = 2
    }
}
