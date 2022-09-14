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
    public ICommand DeleteNoteCommand => new DelegateCommand<NoteModel?>(DeleteNoteCommandHandler);
    public ICommand EditNoteCommand => new DelegateCommand<NoteModel?>(EditNoteCommandHandler);

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

    private async void DeleteNoteCommandHandler(NoteModel? noteModel)
    {
        if (noteModel == null)
        {
            return;
        }

        await ExecuteFuncAsync(async () =>
        {
            await _notesService.DeleteNoteAsync(noteModel.Id);
            Notes.Remove(noteModel);
        });
    }

    private void EditNoteCommandHandler(NoteModel? noteModel)
    {
        if (noteModel == null)
        {
            return;
        }

        var dialogParameters = new DialogParameters { { "note", noteModel } };

        _dialogService.ShowDialog(Constants.Dialogs.EditNoteDialog, dialogParameters,
            (result) => HandleEditNoteDialogResult(noteModel, result));
    }

    #endregion

    #region Private Methods

    private async void LoadNotesAsync()
    {
        await ExecuteFuncAsync(async () =>
        {
            var notes = await _notesService.GetAllNotesAsync();
            var noteModels = _mapper.Map<IEnumerable<Note>, IEnumerable<NoteModel>>(notes);
            Notes = new ObservableCollection<NoteModel>(noteModels);
        });
    }

    private void ToggleLoading()
    {
        _eventAggregator.GetEvent<ProgressBarEvent>().Publish(IsLoading = !IsLoading);
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

    private async void HandleEditNoteDialogResult(NoteModel noteModel, IDialogResult result)
    {
        if (result.Result != ButtonResult.OK)
        {
            return;
        }

        var updatedNoteModel = result.Parameters.GetValue<NoteModel>("note");

        if (noteModel == updatedNoteModel)
        {
            return;
        }

        var note = _mapper.Map<Note>(updatedNoteModel);

        await ExecuteFuncAsync(async () =>
        {
            await _notesService.UpdateNoteAsync(note);

            noteModel.Title = updatedNoteModel.Title;
            noteModel.Content = updatedNoteModel.Content;
        });
    }

    #endregion
}