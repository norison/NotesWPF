using NotesWPF.DataAccess.Models;

namespace NotesWPF.Domain.Services.Notes;

public interface INotesService
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Guid id);
}