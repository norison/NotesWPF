namespace NotesWPF.DataAccess.Services.Serialization;

public interface ISerializationService
{
    string SerializeObject(object? value);
    T? DeserializeObject<T>(string value);
}