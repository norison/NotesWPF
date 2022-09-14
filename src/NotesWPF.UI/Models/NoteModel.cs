using System;
using Prism.Mvvm;

namespace NotesWPF.UI.Models;

public class NoteModel : BindableBase, ICloneable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public object Clone()
    {
        return MemberwiseClone();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((NoteModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Title, Content);
    }

    public static bool operator ==(NoteModel? left, NoteModel? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(NoteModel? left, NoteModel? right)
    {
        return !Equals(left, right);
    }

    private bool Equals(NoteModel other)
    {
        return Id.Equals(other.Id) && Title == other.Title && Content == other.Content;
    }
}