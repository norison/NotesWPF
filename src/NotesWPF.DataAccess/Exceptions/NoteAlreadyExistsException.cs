namespace NotesWPF.DataAccess.Exceptions;

public class NoteAlreadyExistsException : Exception
{
    public NoteAlreadyExistsException(string message) : base(message)
    {
    }
}