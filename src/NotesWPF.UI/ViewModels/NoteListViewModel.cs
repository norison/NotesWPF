using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using FluentValidation;
using NotesWPF.DataAccess.Models;
using NotesWPF.Domain.Services.Notes;
using NotesWPF.UI.Events;
using NotesWPF.UI.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace NotesWPF.UI.ViewModels;

public class NoteListViewModel : BindableBase
{
    #region Private Fields

    private readonly INotesService _notesService;
    private readonly IMapper _mapper;
    private readonly IEventAggregator _eventAggregator;
    private readonly IDialogService _dialogService;

    #endregion

    #region Constructor

    public NoteListViewModel(
        INotesService notesService,
        IMapper mapper,
        IEventAggregator eventAggregator,
        IDialogService dialogService)
    {
        _notesService = notesService;
        _mapper = mapper;
        _eventAggregator = eventAggregator;
        _dialogService = dialogService;

        LoadNotesAsync();
    }

    #endregion

    #region Properties

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string LastError { get; set; } = string.Empty;
    public bool IsLoading { get; set; }
    public ObservableCollection<NoteModel> Notes { get; private set; } = new();

    #endregion

    #region Commands

    public ICommand AddNewNoteCommand => new DelegateCommand(AddNewNoteCommandHandler);
    public ICommand DeleteNoteCommand => new DelegateCommand<Guid?>(DeleteNoteCommandHandler);
    public ICommand EditNoteDialogCommand => new DelegateCommand<NoteModel?>(EditNoteDialogCommandHandler);

    #endregion

    #region Command Handlers

    private async void AddNewNoteCommandHandler()
    {
        await ExecuteFuncAsync(async () =>
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
        });
    }

    private async void DeleteNoteCommandHandler(Guid? noteId)
    {
        if (noteId == null)
        {
            return;
        }

        await ExecuteFuncAsync(async () =>
        {
            await _notesService.DeleteNoteAsync(noteId.Value);
            Notes.Remove(Notes.First(x => x.Id == noteId));
        });
    }

    private void EditNoteDialogCommandHandler(NoteModel? noteModel)
    {
        if (noteModel == null)
        {
            return;
        }

        var dialogParameters = new DialogParameters { { "note", noteModel } };

        // ReSharper disable once AsyncVoidLambda
        _dialogService.ShowDialog(Constants.Dialogs.EditNoteDialog, dialogParameters, async (result) =>
        {
            if (result.Result != ButtonResult.OK)
            {
                return;
            }

            var updatedNote = result.Parameters.GetValue<NoteModel>("note");
            var currentNote = Notes.First(x => x.Id == updatedNote.Id);

            if (currentNote.Equals(updatedNote))
            {
                return;
            }
            
            var note = _mapper.Map<Note>(updatedNote);

            await ExecuteFuncAsync(async () =>
            {
                await _notesService.UpdateNoteAsync(note);
                
                currentNote.Title = updatedNote.Title;
                currentNote.Content = updatedNote.Content;
            });
        });
    }

    #endregion

    #region Private Methods

    private async void LoadNotesAsync()
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

    private void ToggleLoading()
    {
        var evt = _eventAggregator.GetEvent<ProgressBarEvent>();

        evt.Publish(!IsLoading);
        IsLoading = !IsLoading;
    }

    private async Task ExecuteFuncAsync(Func<Task> func)
    {
        try
        {
            ToggleLoading();

            await Task.Delay(2000);
            await func();
        }
        catch (ValidationException exception)
        {
            LastError = exception.Errors.First().ErrorMessage;
        }
        catch (Exception exception)
        {
            LastError = exception.Message;
        }
        finally
        {
            ToggleLoading();
        }
    }

    #endregion
}