namespace Yape.AntiFraud.Strategy
{
    public class AccumulatedPerDayStrategy : IRejectionStrategy
    {
        private readonly decimal _maxAmount;

        public AccumulatedPerDayStrategy(decimal maxAmount)
        {
            _maxAmount = maxAmount;
        }

        public bool Limit(decimal amount)
        {
            //TODO: persistir acumulacion de montos por dia
            
            return _maxAmount > amount;
        }
    }
}
