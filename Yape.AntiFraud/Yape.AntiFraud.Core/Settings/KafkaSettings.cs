namespace Yape.AntiFraud.Core.Settings
{
    public class KafkaSettings
    {
        public required string BootstrapServers { get; set; }
        public required string GroupId { get; set; }
        public required string Topic { get; set; }

    }
}
