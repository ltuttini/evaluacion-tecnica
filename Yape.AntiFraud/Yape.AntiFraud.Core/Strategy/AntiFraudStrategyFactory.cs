namespace Yape.AntiFraud.Strategy
{
    public class AntiFraudStrategyFactory
    {
        private readonly List<IRejectionStrategy> _strategy;

        public AntiFraudStrategyFactory() 
        {
            _strategy = new List<IRejectionStrategy>();
        }
        public void AddStrategy(IRejectionStrategy strategy)
        {
            _strategy.Add(strategy);
        }

        public bool ApplyLimits(decimal amount)
        {
            bool result = false;
            foreach (var estrategia in _strategy)
            {
                estrategia.Limit(amount);
            }

            return result;
        }

    }

}
