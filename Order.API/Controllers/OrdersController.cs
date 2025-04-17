using Core.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Models;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly AppDbContext _context; // normalde bi service katmanı olurdu ama konu o değil şu an.
        private readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            var newOrder = new Models.Order
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address
                {
                    Line = orderCreate.Address.Line,
                    District = orderCreate.Address.District,
                    Province = orderCreate.Address.Province
                },
                CreatedDate = DateTime.UtcNow,
            };

            foreach (var item in orderCreate.OrderItems)
            {
                newOrder.Items.Add(new OrderItem { Count = item.Count, Price = item.Price, ProductId = item.ProductId });
            }

            await _context.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var orderEvent = new OrderCreatedEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                PaymentMessage = new Core.PaymentMessage
                {
                    CardName = orderCreate.Payment.CardName,
                    CardNumber = orderCreate.Payment.CardNumber,
                    CVV = orderCreate.Payment.CVV,
                    Expiration = orderCreate.Payment.Expiration,
                    TotalPrice = orderCreate.OrderItems.Sum(x => x.Price * x.Count)
                }
            };

            foreach (var item in orderCreate.OrderItems)
            {
                orderEvent.OrderItems.Add(new Core.OrderItemMessage { ProductId = item.ProductId, Count = item.Count });
            }

            await _publishEndpoint.Publish(orderEvent); 

            return Ok();
        }
    }
}
