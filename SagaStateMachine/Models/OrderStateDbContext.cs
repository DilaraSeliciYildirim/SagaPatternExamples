using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagaStateMachine.Models
{
    public class OrderStateDbContext : SagaDbContext // MassTransit EF Core lib'e ihtiyaç var.
    {
        public OrderStateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        { 
            get { yield return new OrderStateMap(); } 
        }
    }
}
