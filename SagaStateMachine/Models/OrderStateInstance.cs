using MassTransit;
using System.Text;

namespace SagaStateMachine.Models
{
    public class OrderStateInstance : SagaStateMachineInstance // Her Order'ın kendi state'i olacak
    {
        public Guid CorrelationId { get; set; } // Her Order'ın state'ini ayrı ayrı tutabilmek için böyle bi Id'ye ihtiyaç var.
        public string CurrentState { get; set; }
        public string BuyerId { get; set; }
        public int OrderId { get; set; }

        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            var props = GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var item in props.ToList())
            {
                sb.AppendLine($"{item.Name}:{item.GetValue(this, null)}");
            }

            sb.AppendLine("-----");

            return sb.ToString();
        }
    }
}
