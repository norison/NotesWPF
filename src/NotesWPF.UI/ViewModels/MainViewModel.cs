using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AutoMapper;
using FluentValidation;
using NotesWPF.DataAccess.Models;
using NotesWPF.Domain.Services.Notes;
using NotesWPF.UI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace NotesWPF.UI.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly INotesService _notesService;
    private readonly IMapper _mapper;

    public MainViewModel(INotesService notesService, IMapper mapper)
    {
        _notesService = notesService;
        _mapper = mapper;
    }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string LastError { get; set; } = string.Empty;
    public ObservableCollection<NoteModel> Notes { get; private set; } = new();

    public ICommand LoadNotesCommand => new DelegateCommand(LoadNotesCommandHandler);
    public ICommand AddNewNoteCommand => new DelegateCommand(AddNewNoteCommandHandler);
    public ICommand DeleteNoteCommand => new DelegateCommand<Guid?>(DeleteNoteCommandHandler);

    private async void LoadNotesCommandHandler()
    {
        try
        {
            var notes = await _notesService.GetAllNotesAsync();
            var noteModels = _mapper.Map<IEnumerable<Note>, IEnumerable<NoteModel>>(notes);
            Notes = new ObservableCollection<NoteModel>(noteModels);
        }
        catch (Exception exception)
        {
            LastError = exception.Message;
        }
    }

    private async void AddNewNoteCommandHandler()
    {
        try
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = Title,
                Content = Content
            };

            await _notesService.AddNoteAsync(note);

            Notes.Add(_mapper.Map<NoteModel>(note));
            
            Title = string.Empty;
            Content = string.Empty;
        }
        catch (ValidationException exception)
        {
            LastError = exception.Errors.First().ErrorMessage;
        }
        catch (Exception exception)
        {
            LastError = exception.Message;
        }
    }

    private async void DeleteNoteCommandHandler(Guid? noteId)
    {
        if (noteId == null)
        {
            return;
        }

        try
        {
            await _notesService.DeleteNoteAsync(noteId.Value);
            Notes.Remove(Notes.First(x => x.Id == noteId));
        }
        catch (Exception exception)
        {
            LastError = exception.Message;
        }
    }
}