using SkinkiDriverApi.Models;
using SkinkiDriverApi.ValueObjects;

namespace SkinkiDriverApi.Interfaces
{
    public interface ISkinkiDriverService
    {
        public Task<BusinessResult<List<UploadFileResponse>>> UploadAsync(UploadFileRequest uploadFile);
        public Task<BusinessResult<FileStream>> GetAsync(string path);
        public Task<BusinessResult<List<UploadFileResponse>>> GetFilesAsync(string path);
        public Task<BusinessResult<List<UploadFileResponse>>> GetFilesWithBaseUrlAsync(string baseUrl, string path);
    }
}
