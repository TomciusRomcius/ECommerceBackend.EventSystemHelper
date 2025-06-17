using Confluent.Kafka;
using EventSystemHelper.Interfaces;
using EventSystemHelper.Kafka.Utils;
using System.Net;
namespace EventSystemHelper.Kafka.Services
{
    public class KafkaEventProducer : IEventProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaEventProducer(KafkaConfiguration kafkaConfiguration)
        {
            var cfg = new ProducerConfig
            {
                BootstrapServers = kafkaConfiguration.Servers,
                ClientId = Dns.GetHostName()
            };
            _producer = new ProducerBuilder<Null, string>(cfg).Build();
        }

        public async Task ProduceEventAsync(string topic, string jsonMessage, CancellationToken cancellationToken)
        {
            var message = new Message<Null, string>
            {
                Value = jsonMessage
            };

            await _producer.ProduceAsync(topic, message, cancellationToken);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
