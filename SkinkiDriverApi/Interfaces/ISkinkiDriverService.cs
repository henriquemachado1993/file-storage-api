using SkinkiDriverApi.Models;
using SkinkiDriverApi.ValueObjects;

namespace SkinkiDriverApi.Interfaces
{
    public interface ISkinkiDriverService
    {
        public Task<BusinessResult<List<UploadFileResponse>>> UploadAsync(UploadFileRequest uploadFile);
        public Task<BusinessResult<UploadFileResponse>> GetAsync(string path);
        public Task<BusinessResult<List<UploadFileResponse>>> GetFilesAsync(string path);
    }
}
