using NotesWPF.Domain.Services.Notes;
using Prism.Mvvm;

namespace NotesWPF.UI.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly INotesService _notesService;

    public MainViewModel(INotesService notesService)
    {
        _notesService = notesService;
    }
}