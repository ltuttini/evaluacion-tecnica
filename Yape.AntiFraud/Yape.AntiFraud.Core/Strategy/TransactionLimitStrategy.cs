namespace Yape.AntiFraud.Strategy
{
    public class TransactionLimitStrategy : IRejectionStrategy
    {
        private readonly decimal _maxAmount;

        public TransactionLimitStrategy(decimal maxAmount)
        {
            _maxAmount = maxAmount;
        }

        public bool Limit(decimal amount)
        {
            return _maxAmount > amount;
        }
    }
}
