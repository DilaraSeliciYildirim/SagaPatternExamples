using MassTransit;
using Microsoft.AspNetCore.Mvc;
using O.Core;
using O.Core.Events;
using O.Core.Interfaces;
using O.Order.API.DTOs;
using O.Order.API.Models;

namespace O.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly AppDbContext _context; // normalde bi service katmanı olurdu ama konu o değil şu an.
        //private readonly IPublishEndpoint _publishEndpoint; // bunu diğer Saga Pattern'de kullandık
        private readonly ISendEndpointProvider _sendEndpointProvider; // burada Event'i sadece SagaStateMachine dinliycek o yüzden Send

        public OrdersController(AppDbContext context, ISendEndpointProvider sendEndpointProvider)
        {
            _context = context;
            _sendEndpointProvider = sendEndpointProvider;
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

            var orderEvent = new OrderCreatedRequestEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage
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
                orderEvent.OrderItems.Add(new OrderItemMessage { ProductId = item.ProductId, Count = item.Count });
            }

            // await _publishEndpoint.Publish(orderEvent); 

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.OrderSaga}"));
            // uri massTransit'in standardına göre bu şekilde yazılıyor.

            await sendEndpoint.Send<IOrderCreatedRequestEvent>(orderEvent);

            return Ok();
        }
    }
}
