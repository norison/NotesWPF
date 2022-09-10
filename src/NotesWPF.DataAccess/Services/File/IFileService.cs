namespace NotesWPF.DataAccess.Services.File;

public interface IFileService
{
    Task<string> ReadAllTextAsync(string path);
    Task WriteAllTextAsync(string path, string? contents);
}