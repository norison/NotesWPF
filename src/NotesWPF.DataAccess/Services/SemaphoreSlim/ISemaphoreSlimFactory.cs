namespace NotesWPF.DataAccess.Services.SemaphoreSlim;

public interface ISemaphoreSlimFactory
{
    ISemaphoreSlim Create(int initialCount);
    ISemaphoreSlim Create(int initialCount, int maxCount);
}