using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Services;

namespace Infrastructure.CommonServices;

public class SystemTextJsonService : ISerializerService
{
    public T Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<T>(text);
    }

    public string Serialize<T>(T obj)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        return JsonSerializer.Serialize(obj, options);
    }

    public string Serialize<T>(T obj, Type type)
    {
        var options = new JsonSerializerOptions();

        return JsonSerializer.Serialize(obj, type, options);
    }
}