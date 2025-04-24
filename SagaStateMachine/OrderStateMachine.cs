using MassTransit;
using O.Core.Interfaces;
using SagaStateMachine.Models;

namespace SagaStateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public State OrderCreated { get; private set; }
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));

            // OrderCreatedREquest event'i geldiğinde, State'teki (x) OrderId ile Event'den gelen (z) OrderId'yi karşılaştır
            // eğer uyuşmazlarsa demek ki yeni bir Order, o zaman Guid id ile yeni bi satır oluştur


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
               .TransitionTo(OrderCreated)
               .Then(context => { Console.WriteLine($"After OrderCreatedRequestEvent: {context.Saga}"); }));
        }
    }
}
