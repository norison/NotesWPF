using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace NotesWPF.DataAccess.Services.Serialization;

[ExcludeFromCodeCoverage]
public class SerializationService : ISerializationService
{
    public string SerializeObject(object? value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public T? DeserializeObject<T>(string value)
    {
        return JsonConvert.DeserializeObject<T>(value);
    }
}