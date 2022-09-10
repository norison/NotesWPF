using System;
using Prism.Mvvm;

namespace NotesWPF.UI.Models;

public class NoteModel : BindableBase
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}