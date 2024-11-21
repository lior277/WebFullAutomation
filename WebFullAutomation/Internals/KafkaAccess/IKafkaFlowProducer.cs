using LSports.FixtureManager.Gatekeeper.Abstractions.Entities;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public interface IKafkaFlowProducer
{
    void Produce(ProviderFixture providerFixture);
}