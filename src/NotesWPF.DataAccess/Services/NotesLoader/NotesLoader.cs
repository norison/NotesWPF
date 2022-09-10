using System.Runtime.Serialization;
using NotesWPF.DataAccess.Models;
using NotesWPF.DataAccess.Services.File;
using NotesWPF.DataAccess.Services.Serialization;

namespace NotesWPF.DataAccess.Services.NotesLoader;

public class NotesLoader : INotesLoader
{
    private readonly IFileService _fileService;
    private readonly ISerializationService _serializationService;

    public NotesLoader(IFileService fileService, ISerializationService serializationService)
    {
        _fileService = fileService;
        _serializationService = serializationService;
    }

    public async Task<IEnumerable<Note>> LoadNotesAsync(string path)
    {
        if (!_fileService.Exists(path))
        {
            return new List<Note>();
        }

        var json = await _fileService.ReadAllTextAsync(path);
        var notes = _serializationService.DeserializeObject<IEnumerable<Note>>(json);

        if (notes == null)
        {
            throw new SerializationException("Failed to deserialize notes from file.");
        }

        return notes;
    }

    public async Task SaveNotesAsync(string path, IEnumerable<Note> notes)
    {
        var json = _serializationService.SerializeObject(notes);
        await _fileService.WriteAllTextAsync(path, json);
    }
}