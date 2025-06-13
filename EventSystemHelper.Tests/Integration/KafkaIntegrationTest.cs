using Confluent.Kafka;
using EventSystemHelper.Interfaces.Utils;
using EventSystemHelper.Services;
using System.Text.Json;

namespace EventSystemHelper.Tests.Integration
{
    class MessageType
    {
        public required string Item { get; set; }
    }

    public class KafkaIntegrationTest
    {
        /** Basic integration test where we setup a consumer and
         * producer and check if the consumer gets the message
         */
        [Fact]
        public async Task KafkaEventConsumerAndKafkaEventConsumer_ShouldProduceAndConsumeEvents()
        {
            MessageType message = new()
            {
                Item = "a"
            };

            var kafkaCfg = new KafkaConfiguration("localhost:9093");
            var topic = "test-topic";

            using var adminClient = new AdminClientBuilder(new AdminClientConfig
            {
                BootstrapServers = kafkaCfg.Servers,
            }).Build();

            try
            {
                await adminClient.DeleteTopicsAsync([topic]);
            }
            catch { }

            KafkaEventConsumer consumer = new KafkaEventConsumer(kafkaCfg, AutoOffsetReset.Earliest, "group-id", topic);
            KafkaEventProducer producer = new KafkaEventProducer(kafkaCfg);

            var pcts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var messageJson = JsonSerializer.Serialize(message);
            await producer.ProduceEventAsync(topic, messageJson, pcts.Token);

            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            MessageType? retrieved = consumer.Consume<MessageType>(cts.Token);
            Assert.NotNull(retrieved);
            Assert.Equal(message.Item, retrieved.Item);
        }
    }
}
