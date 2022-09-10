namespace NotesWPF.DataAccess.Exceptions;

public class NoteNotFoundException : Exception
{
    public NoteNotFoundException(string message) : base(message)
    {
    }
}