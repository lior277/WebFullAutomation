using KafkaFlow;
using LSports.FixtureManager.Gatekeeper.Abstractions.Entities;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public class KafkaFlowProducer : IKafkaFlowProducer
{
    private readonly IMessageProducer<KafkaFlowProducer> _producer;


    public KafkaFlowProducer(IMessageProducer<KafkaFlowProducer> producer)
    {
        _producer = producer;
    }

    public void Produce(ProviderFixture providerFixture)
    {
        _producer.Produce(
            $"{providerFixture.RobotId}_{providerFixture.ProviderFixtureId}", providerFixture);
    }
}