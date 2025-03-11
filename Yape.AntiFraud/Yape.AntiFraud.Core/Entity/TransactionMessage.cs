namespace Yape.AntiFraud.Core.Entity
{
    public class TransactionMessage
    {
        public int Id { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }

        public State State { get; set; }
        public int Value { get; set; }
    }

    public enum State
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
