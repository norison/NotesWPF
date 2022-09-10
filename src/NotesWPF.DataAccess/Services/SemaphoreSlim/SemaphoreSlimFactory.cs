using System.Diagnostics.CodeAnalysis;

namespace NotesWPF.DataAccess.Services.SemaphoreSlim;

[ExcludeFromCodeCoverage]
public class SemaphoreSlimFactory : ISemaphoreSlimFactory
{
    public ISemaphoreSlim Create(int initialCount)
    {
        return new SemaphoreSlimWrapper(initialCount);
    }

    public ISemaphoreSlim Create(int initialCount, int maxCount)
    {
        return new SemaphoreSlimWrapper(initialCount, maxCount);
    }
}