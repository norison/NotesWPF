using NotesWPF.DataAccess.Models;

namespace NotesWPF.DataAccess;

public interface INotesRepository
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
}