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

        [HttpGet("get-file/{folder2}/{folder3}/{folder4}/{file}")]
        public async Task<IActionResult> Get([FromRoute]string folder2,[FromRoute]string folder3,[FromRoute]string folder4,[FromRoute]string file)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var imagePath = Path.Combine(currentDirectory, "Uploads", folder2, folder3, folder4, file);
            var fs= new FileStream(imagePath, FileMode.Open, FileAccess.Read);             
            return new FileStreamResult(fs, "image/jpeg");
            
        }

        [HttpPost("get-files")]
        public async Task<IActionResult> GetFiles([FromBody] GetRequest path)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var imagePath = Path.Combine(currentDirectory, "Uploads", path.Path);
            var files = Directory.GetFiles(imagePath);

            var paths = new List<string>();

            foreach (var filePath in Directory.EnumerateFiles(imagePath, "*.jpeg"))
            {
                
                paths.Add($"http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file{filePath.Split("Uploads")[1]}");
            }


            return Ok(paths);
        }
    }
}
