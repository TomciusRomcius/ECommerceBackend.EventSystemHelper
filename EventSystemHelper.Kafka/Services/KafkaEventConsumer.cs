using Confluent.Kafka;
using EventSystemHelper.Interfaces;
using EventSystemHelper.Kafka.Utils;
using Newtonsoft.Json;

namespace EventSystemHelper.Kafka.Services
{
    public class KafkaEventConsumer : IEventConsumser
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaEventConsumer(KafkaConfiguration kafkaConfiguration, AutoOffsetReset autoOffsetReset, string groupId, string topic)
        {
            var cfg = new ConsumerConfig
            {
                BootstrapServers = kafkaConfiguration.Servers,
                GroupId = groupId,
                AutoOffsetReset = autoOffsetReset,
            };

            _consumer = new ConsumerBuilder<Ignore, string>(cfg).Build();
            _consumer.Subscribe(topic);
        }

        public T? Consume<T>(CancellationToken cancellationToken)
        {
            string message = _consumer.Consume(cancellationToken).Message.Value;
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
