using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using LSports.Core.Common.Extensions;
using LSports.FixtureManager.Abstractions.ProviderEvents.DTOs;
using MetaversAutomation.Internals.Factories;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public class KafkaMessageMediator : IKafkaMessageMediator
{
    private readonly ConcurrentDictionary<string,
        TaskCompletionSource<ProviderEventDto>> _taskCompletionSources = new();

    public TaskCompletionSource<ProviderEventDto> Subscribe(string searchingKeyForKafka)
    {
        return _taskCompletionSources.GetOrAdd(searchingKeyForKafka, _ =>
            new TaskCompletionSource<ProviderEventDto>());
    }

    public void UnSubscribe(string searchingKeyForKafka)
    {
        _taskCompletionSources.Remove(searchingKeyForKafka, out _);
    }

    public bool SetFixture(ProviderEventDto providerEventContainerDto)
    {
        if (providerEventContainerDto == null || string.IsNullOrEmpty(providerEventContainerDto.ProviderFixtureId))
        {
            // Log error or handle null DTO
            // Example: _logger.LogError("Received null or invalid ProviderEventDto.");
            return false;
        }

        var taskCompletionSource = _taskCompletionSources
            .GetOrAdd(providerEventContainerDto.ProviderFixtureId, _ =>
                new TaskCompletionSource<ProviderEventDto>());

        if (taskCompletionSource.Task.IsCompleted)
        {
            Console.WriteLine($"Task for key  is already completed., {providerEventContainerDto.ProviderFixtureId}");
            
            return false;
        }

        var success = taskCompletionSource.TrySetResult(providerEventContainerDto);
        if (!success)
        {
            Console.WriteLine($"Failed to set result for key., {providerEventContainerDto.ProviderFixtureId}");
        }

        return success;
    }


    public T ChangeContext<T>() where T : class
    {
        return ApplicationFactory.ChangeContext<T>();
    }
}