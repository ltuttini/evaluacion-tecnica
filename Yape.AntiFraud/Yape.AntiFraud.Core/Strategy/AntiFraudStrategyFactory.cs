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
            bool result = true;
            foreach (var estrategia in _strategy)
            {
                result = estrategia.Limit(amount);

                if(!result)
                    break;
            }

            return result;
        }

    }

}
