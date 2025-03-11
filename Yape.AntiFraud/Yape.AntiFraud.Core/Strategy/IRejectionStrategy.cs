namespace Yape.AntiFraud.Strategy
{
    public interface IRejectionStrategy
    {
        bool Limit(decimal amount);
    }
}
