// Assuming IKafkaBus inherits from IServiceProvider

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaFlow;
using MetaversAutomation.Internals.DAL.ConfigManager;
using MetaversAutomation.Internals.Factories;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public class KafkaFlowConsumer : IKafkaFlowConsumer
{
    public async Task StartConsuming()
    {
        var consumer = ApplicationFactory
            .ChangeContext<IServiceProvider>()
            .CreateKafkaBus();

        await consumer.StartAsync();
        await Task.Delay(8000); // Wait for 8 seconds
    }

    public async Task StopConsuming()
    {
        await
            ApplicationFactory
                .ChangeContext<IServiceProvider>()
                .CreateKafkaBus()
                .StopAsync();
    }

    public async Task DeleteConsumerGroup()
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = Configs.BootstrapServers,
            SocketTimeoutMs = 10000,
            SecurityProtocol = SecurityProtocol.Ssl
        };

        using var adminClient = new AdminClientBuilder(config).Build();

        try
        {
            // Delete the consumer group
            await adminClient.DeleteGroupsAsync(new List<string> { Configs.GroupId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting consumer group: {ex.Message}");
        }
    }

    public T ChangeContext<T>() where T : class
    {
        return ApplicationFactory.ChangeContext<T>();
    }
}