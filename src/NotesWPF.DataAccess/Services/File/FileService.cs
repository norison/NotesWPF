using System.Diagnostics.CodeAnalysis;

namespace NotesWPF.DataAccess.Services.File;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
    public async Task<string> ReadAllTextAsync(string path)
    {
        return await System.IO.File.ReadAllTextAsync(path);
    }

    public async Task WriteAllTextAsync(string path, string? contents)
    {
        await System.IO.File.WriteAllTextAsync(path, contents);
    }
}