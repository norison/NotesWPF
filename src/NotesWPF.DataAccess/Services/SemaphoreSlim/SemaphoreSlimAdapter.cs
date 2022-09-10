using System.Diagnostics.CodeAnalysis;

namespace NotesWPF.DataAccess.Services.SemaphoreSlim;

[ExcludeFromCodeCoverage]
public class SemaphoreSlimAdapter : System.Threading.SemaphoreSlim, ISemaphoreSlim
{
    public SemaphoreSlimAdapter(int initialCount) : base(initialCount)
    {
    }

    public SemaphoreSlimAdapter(int initialCount, int maxCount) : base(initialCount, maxCount)
    {
    }
}