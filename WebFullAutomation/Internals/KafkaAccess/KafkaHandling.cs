// Assuming IKafkaBus inherits from IServiceProvider

using System;
using System.Threading.Tasks;
using LSports.FixtureManager.Abstractions.ProviderEvents.DTOs;
using LSports.FixtureManager.Abstractions.ProviderEvents.Flow;
using LSports.FixtureManager.Gatekeeper.Abstractions.Entities;
using MetaversAutomation.Internals.ExtensionsMethods;
using MetaversAutomation.Internals.Factories;
using MetaversAutomation.Internals.Helpers;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public class KafkaHandling : IKafkaHandling
{
    public async Task<ProviderEventDto> ProduceAndConsumeAsync(ProviderFixture providerFixture,
        string providerFixtureId, FixtureCreationFailureReason
            expectedFixtureCreationFailureReason,
        int WaitWithTimeoutInSeconds = DataRep.KafkaWaitWithTimeoutInSeconds)

    {
        ProviderEventDto providerEventDto = null;

        try
        {
            // subscribe the provider Fixture Id to the task
            var result = ApplicationFactory
                .ChangeContext<IKafkaMessageMediator>()
                .Subscribe(providerFixtureId)
                .WaitWithTimeout((int)TimeSpan
                    .FromSeconds(WaitWithTimeoutInSeconds)
                    .TotalMilliseconds);

            // produce to kafka
            // the consume happens in setUpClass
            ApplicationFactory
                .ChangeContext<IKafkaFlowProducer>()
                .Produce(providerFixture);

            // wait for the consumer to complete
            providerEventDto = await result;

            if (!CheckReason(providerEventDto, expectedFixtureCreationFailureReason))
                throw new Exception();

            return providerEventDto;
        }
        catch (Exception)
        {
            string exceptionMessage;

            if (providerEventDto == null)
            {
                exceptionMessage = $"providerEventDto is null and Provider Fixture Id  {providerFixtureId}";
            }
            else
            {
                exceptionMessage = ($" actual Fixture Creation Failure Reason:" +
                                    $" {providerEventDto?.FixtureCreationFailureReason}",
                                    $" actual Provider Fixture Id  {providerFixtureId}",
                    " expected Fixture Creation Failure Reason: none").ToString();
            }

            throw new Exception(exceptionMessage);
        }
    }

    private bool CheckReason(ProviderEventDto providerEventDto,
        FixtureCreationFailureReason expectedFixtureCreationFailureReason)
    {
        return providerEventDto.FixtureCreationFailureReason
               == expectedFixtureCreationFailureReason;
    }

    public async Task<ProviderEventDto> WaitForMessagePipe(string providerFixtureId,
        FixtureCreationFailureReason expectedFixtureCreationFailureReason,
        int WaitWithTimeoutInSeconds = DataRep.KafkaWaitWithTimeoutInSeconds)
    {
        ProviderEventDto providerEventDto = null;

        try
        {
            // subscribe the provider Fixture Id to the task
            var result = ApplicationFactory
                .ChangeContext<IKafkaMessageMediator>()
                .Subscribe(providerFixtureId)
                .WaitWithTimeout((int)TimeSpan
                    .FromSeconds(WaitWithTimeoutInSeconds)
                    .TotalMilliseconds);

            // wait for the consumer to complete
            providerEventDto = await result;
            
            if (!CheckReason(providerEventDto, expectedFixtureCreationFailureReason))
                throw new Exception();
            
            return providerEventDto;
        }
        catch (Exception)
        {
            string exceptionMessage;
            
            if (providerEventDto == null)
            {
                exceptionMessage = " provider Event Dto is null";
            }
            else
            {
                exceptionMessage = ($" actual Fixture Creation Failure Reason:" +
                                    $" {providerEventDto.FixtureCreationFailureReason}",
                    " expected Fixture Creation Failure Reason: none").ToString();
            }

            throw new Exception(exceptionMessage);
        }
    }
}