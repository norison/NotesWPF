namespace NotesWPF.DataAccess.Services.SemaphoreSlim;

public interface ISemaphoreSlim
{
    Task WaitAsync();
    int Release();
}