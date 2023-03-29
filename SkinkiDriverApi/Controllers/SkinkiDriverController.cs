using Microsoft.AspNetCore.Mvc;
using SkinkiDriverApi.Interfaces;
using SkinkiDriverApi.Models;
using SkinkiDriverApi.ValueObjects;

namespace SkinkiDriverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkinkiDriverController : ControllerBase
    {
        private readonly ISkinkiDriverService _driverService;

        public SkinkiDriverController(ISkinkiDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpPost("upload")]
        public async Task<BusinessResult<List<UploadFileResponse>>> Upload(UploadFileRequest request)
        {
            return await _driverService.UploadAsync(request);
        }

        [HttpPost("get-file")]
        public async Task<BusinessResult<UploadFileResponse>> Get(GetRequest request)
        {
            return await _driverService.GetAsync(request.Path, request.NameFile);
        }

        [HttpPost("get-files")]
        public async Task<BusinessResult<List<UploadFileResponse>>> GetFiles(GetRequest request)
        {
            return await _driverService.GetFilesAsync(request.Path);
        }
    }
}
