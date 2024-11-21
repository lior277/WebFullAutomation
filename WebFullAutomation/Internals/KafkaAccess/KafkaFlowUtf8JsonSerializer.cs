using System;
using System.IO;
using System.Threading.Tasks;
using KafkaFlow;
using Utf8Json;

namespace MetaversAutomation.Internals.DAL.KafkaAccess;

public class KafkaFlowUtf8JsonSerializer : ISerializer, IDeserializer
{
    public Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        return JsonSerializer.NonGeneric.DeserializeAsync(type, input);
    }

    public Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
        return JsonSerializer.NonGeneric.SerializeAsync(output, message);
    }
}