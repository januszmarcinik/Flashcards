﻿using Flashcards.Core;

namespace Flashcards.Infrastructure.AzureServiceBus
{
    public class AzureServiceBusSettings : ISettings
    {
        public string HostName { get; set; }
        public string QueueName { get; set; }
    }
}