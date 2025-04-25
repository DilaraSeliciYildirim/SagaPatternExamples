using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O.Core.Interfaces
{
    // Bu coralatedBy'dan miras almıyor çünkü zaten state machine'de fırlatılmıyo
    public interface IOrderCreatedRequestEvent // order OrderApi'de olustuktan sonra StateMachine'e gidecek event
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public PaymentMessage Payment { get; set; }
    }
}
