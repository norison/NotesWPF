using System;
using System.Windows.Input;
using NotesWPF.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace NotesWPF.UI.Dialogs.EditNoteDialog;

public class EditNoteDialogViewModel : BindableBase, IDialogAware
{
    #region Properties

    public NoteModel Note { get; private set; } = new();

    #endregion

    #region Commands

    public ICommand CancelCommand => new DelegateCommand(CancelCommandHandler);
    public ICommand SaveChangesCommand => new DelegateCommand(SaveChangesCommandHandler);

    #endregion

    #region Command Handlers

    private void CancelCommandHandler()
    {
        var dialogResult = new DialogResult(ButtonResult.Cancel);
        RequestClose?.Invoke(dialogResult);
    }

    private void SaveChangesCommandHandler()
    {
        var dialogParameters = new DialogParameters { { "note", Note } };
        var dialogResult = new DialogResult(ButtonResult.OK, dialogParameters);
        RequestClose?.Invoke(dialogResult);
    }

    #endregion

    #region IDialogAware

    public string Title => "Edit Note";
    public event Action<IDialogResult>? RequestClose;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        var note = parameters.GetValue<NoteModel>("note");
        Note = (NoteModel)note.Clone();
    }

    #endregion
}