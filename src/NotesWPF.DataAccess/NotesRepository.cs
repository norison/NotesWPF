using NotesWPF.DataAccess.Exceptions;
using NotesWPF.DataAccess.Models;
using NotesWPF.DataAccess.Services.NotesLoader;
using NotesWPF.DataAccess.Services.SemaphoreSlim;
using NotesWPF.DataAccess.Settings;

namespace NotesWPF.DataAccess;

public class NotesRepository : INotesRepository
{
    private readonly INotesLoader _notesLoader;
    private readonly ISemaphoreSlim _semaphoreSlim;
    private readonly RepositorySettings _repositorySettings;

    private IDictionary<Guid, Note>? _notes;

    public NotesRepository(
        INotesLoader notesLoader,
        ISemaphoreSlimFactory semaphoreSlimFactory,
        RepositorySettings repositorySettings)
    {
        _notesLoader = notesLoader;
        _semaphoreSlim = semaphoreSlimFactory.Create(1);
        _repositorySettings = repositorySettings;
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        var notes = await LoadNotesAsync();
        return notes.Values;
    }

    public async Task AddNoteAsync(Note note)
    {
        var notes = await LoadNotesAsync();
        if (notes.ContainsKey(note.Id))
        {
            throw new NoteAlreadyExistsException($"Note already exists. ID: {note.Id}");
        }

        notes.Add(note.Id, note);
        await SaveNotesAsync();
    }

    public async Task UpdateNoteAsync(Note note)
    {
        var notes = await LoadNotesAsync();
        if (!notes.ContainsKey(note.Id))
        {
            throw new NoteNotFoundException($"Note was not found. ID: {note.Id}");
        }

        notes[note.Id] = note;
        await SaveNotesAsync();
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        var notes = await LoadNotesAsync();
        if (!notes.ContainsKey(id))
        {
            throw new NoteNotFoundException($"Note was not found. ID: {id}");
        }

        notes.Remove(id);
        await SaveNotesAsync();
    }

    private async Task<IDictionary<Guid, Note>> LoadNotesAsync()
    {
        if (_notes != null)
        {
            return _notes;
        }

        try
        {
            await _semaphoreSlim.WaitAsync();

            if (_notes != null)
            {
                return _notes;
            }

            var newNotes = await _notesLoader.LoadNotesAsync(_repositorySettings.Path);
            return _notes = newNotes.ToDictionary(x => x.Id);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task SaveNotesAsync()
    {
        if (_notes != null)
        {
            await _notesLoader.SaveNotesAsync(_repositorySettings.Path, _notes.Values);
        }
    }
}