using System.Diagnostics.CodeAnalysis;

namespace NotesWPF.DataAccess.Services.SemaphoreSlim;

[ExcludeFromCodeCoverage]
public class SemaphoreSlimWrapper : System.Threading.SemaphoreSlim, ISemaphoreSlim
{
    public SemaphoreSlimWrapper(int initialCount) : base(initialCount)
    {
    }

    public SemaphoreSlimWrapper(int initialCount, int maxCount) : base(initialCount, maxCount)
    {
    }
}