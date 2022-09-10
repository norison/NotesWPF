using FluentValidation;
using NotesWPF.DataAccess.Models;

namespace NotesWPF.Domain.Validators;

public class NoteValidator : AbstractValidator<Note>
{
    public NoteValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
    }
}