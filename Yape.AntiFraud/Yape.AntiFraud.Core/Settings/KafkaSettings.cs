﻿namespace Yape.AntiFraud.Core.Settings
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }

    }
}
