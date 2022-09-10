using NotesWPF.DataAccess.Models;

namespace NotesWPF.DataAccess.Services.NotesLoader;

public interface INotesLoader
{
    Task<IEnumerable<Note>> LoadNotesAsync(string path);
    Task SaveNotesAsync(string path, IEnumerable<Note> notes);
}