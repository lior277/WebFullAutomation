using System;
using System.Threading.Tasks;
using KafkaFlow;
using LSports.FixtureManager.Abstractions.ProviderEvents.DTOs;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public class KafkaMessageHandler : IMessageHandler<ProviderEventDto>
{
    private readonly IKafkaMessageMediator _kafkaMessageMediator;

    public KafkaMessageHandler(IKafkaMessageMediator kafkaMessageMediator)
    {
        _kafkaMessageMediator = kafkaMessageMediator;
    }

    public Task Handle(IMessageContext context,
        ProviderEventDto providerEventDto)
    {
        try
        {
            if (_kafkaMessageMediator.SetFixture(providerEventDto))
                return Task.CompletedTask;
            Console.WriteLine("Failed to set fixture");

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }
}