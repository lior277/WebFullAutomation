using System.Threading.Tasks;
using LSports.FixtureManager.Abstractions.ProviderEvents.DTOs;
using LSports.FixtureManager.Abstractions.ProviderEvents.Flow;
using LSports.FixtureManager.Gatekeeper.Abstractions.Entities;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public interface IKafkaHandling
{
    Task<ProviderEventDto> ProduceAndConsumeAsync(ProviderFixture providerFixture,
        string providerFixtureId, FixtureCreationFailureReason
            expectedFixtureCreationFailureReason, int WaitWithTimeoutInMilliseconds = 50);

    Task<ProviderEventDto> WaitForMessagePipe(string providerFixtureId,
        FixtureCreationFailureReason expectedFixtureCreationFailureReason,
        int WaitWithTimeoutInMilliseconds = 50);
}