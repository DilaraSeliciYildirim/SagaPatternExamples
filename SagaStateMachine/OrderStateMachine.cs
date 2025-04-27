using MassTransit;
using O.Core;
using O.Core.Events;
using O.Core.Interfaces;
using SagaStateMachine.Models;

namespace SagaStateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; } // SagaStateMachine'i tetikliyecek event.
        public Event<IStockReservedEvent> StockReservedEvent { get; set; } // StockAPI'den gelicek event, Saga dinleyecek..
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; } // PaymentAPI'den gelicek event, Saga dinleyecek..
        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; } 
        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentCompleted { get; private set; }
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            // OrderCreatedREquest event'i geldiğinde, State'teki (x) OrderId ile Event'den gelen (z) OrderId'yi karşılaştır
            // eğer uyuşmazlarsa demek ki yeni bir Order, o zaman Guid id ile yeni bi satır oluştur

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));


            // State initial iken, OrderCreatedRequestEvent geldiğinde, event'den gelen datayı (message) DB'deki dataya (Saga)
            // assign edip state'i OrderCreated'a alıyoruz.

            During(Initial,
                When(OrderCreatedRequestEvent)
               .Then(context =>
               {
                   context.Saga.OrderId = context.Message.OrderId;
                   context.Saga.BuyerId = context.Message.BuyerId;
                   context.Saga.CreatedDate = DateTime.Now;

                   context.Saga.CardNumber = context.Message.Payment.CardNumber;
                   context.Saga.CardName = context.Message.Payment.CardName;
                   context.Saga.CVV = context.Message.Payment.CVV;
                   context.Saga.Expiration = context.Message.Payment.Expiration;
                   context.Saga.TotalPrice = context.Message.Payment.TotalPrice;

               })
               .Then(context => { Console.WriteLine($"Before OrderCreatedRequestEvent: {context.Saga}"); })
               .Publish(context => new OrderCreatedEvent(context.Saga.CorrelationId) { OrderItems = context.Message.OrderItems })
               .TransitionTo(OrderCreated)
               .Then(context => { Console.WriteLine($"After OrderCreatedRequestEvent: {context.Saga}"); }));

            During(OrderCreated, 
                When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{RabbitMQSettings.StockReservedRequestPaymentQueueName}"), context => 
                new StockReservedRequestPaymentEvent(context.Saga.CorrelationId)
                { OrderItems = context.Message.OrderItems, // burdaki message When kısmındaki event'den gelen message.
                  Payment = new PaymentMessage()
                  {
                      CardName = context.Saga.CardName,
                      CardNumber = context.Saga.CardNumber,
                      CVV = context.Saga.CVV,
                      Expiration = context.Saga.Expiration,
                      TotalPrice = context.Saga.TotalPrice,
                  }
                })
                .Then(context => { Console.WriteLine($"After StockReservedEvent: {context.Saga}"); }),
                
                When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Publish(context => new OrderRequestFailedEvent() 
                { OrderId = context.Saga.OrderId, Reason = context.Message.Reason }));

            During(StockReserved,
                When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(context => new OrderRequestCompletedEvent() { OrderId = context.Saga.OrderId })
                .Then(context => { Console.WriteLine($"After PaymentCompletedEvent: {context.Saga}"); })
                .Finalize()); // Bu adım önemli, artık işlem bitti state "final" olacak
        }
    }
}
