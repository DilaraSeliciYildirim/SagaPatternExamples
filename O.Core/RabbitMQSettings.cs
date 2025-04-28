namespace O.Core
{
    public static class RabbitMQSettings
    {
        public const string StockRollbackEventQueue = "stock-rollback-queue";
        public const string OrderSaga = "order-saga-queue";
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockReservedRequestPaymentQueueName = "payment-stock-reserved";
    }
}
