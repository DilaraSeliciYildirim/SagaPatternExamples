using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class RabbitMQSettings
    {
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
    }
}
