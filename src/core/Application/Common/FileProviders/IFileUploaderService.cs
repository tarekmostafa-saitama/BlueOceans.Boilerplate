using Shared.ServiceContracts;

namespace Application.Common.FileProviders;

public interface IFileUploaderService: IScopedService
{
    Task<string> UploadAsync(Stream data, string fileName, string folderName = "uploads", bool compress = false);
    void ClearDirectory(string folderName = "uploads");
}