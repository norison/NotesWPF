using System.Diagnostics.CodeAnalysis;

namespace NotesWPF.DataAccess.Services.SemaphoreSlim;

[ExcludeFromCodeCoverage]
public class SemaphoreSlimFactory : ISemaphoreSlimFactory
{
    public ISemaphoreSlim Create(int initialCount)
    {
        return new SemaphoreSlimAdapter(initialCount);
    }

    public ISemaphoreSlim Create(int initialCount, int maxCount)
    {
        return new SemaphoreSlimAdapter(initialCount, maxCount);
    }
}