using FluentValidation;
using NotesWPF.DataAccess;
using NotesWPF.DataAccess.Models;

namespace NotesWPF.Domain.Services.Notes;

public class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;
    private readonly IValidator<Note> _noteValidator;

    public NotesService(INotesRepository notesRepository, IValidator<Note> noteValidator)
    {
        _notesRepository = notesRepository;
        _noteValidator = noteValidator;
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        return await _notesRepository.GetAllNotesAsync();
    }

    public async Task AddNoteAsync(Note note)
    {
        await _noteValidator.ValidateAsync(note);
        await _notesRepository.AddNoteAsync(note);
    }

    public async Task UpdateNoteAsync(Note note)
    {
        await _noteValidator.ValidateAsync(note);
        await _notesRepository.UpdateNoteAsync(note);
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return;
        }

        await _notesRepository.DeleteNoteAsync(id);
    }
}