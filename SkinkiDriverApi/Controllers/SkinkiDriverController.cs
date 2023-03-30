using Microsoft.AspNetCore.Http;
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
            var result = await _driverService.GetAsync(Path.Combine(folder2, folder3, folder4, file));

            if (!result.IsValid)
                return BadRequest(result.Messages.Select(x => x.Message).ToList());

            return new FileStreamResult(result.Data, "image/jpeg");
        }

        [HttpPost("get-files")]
        public async Task<IActionResult> GetFiles([FromBody] GetRequest path)
        {
            var urlBase = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var result = await _driverService.GetFilesWithBaseUrlAsync($"{urlBase}/SkinkiDriver/get-file", path.Path);

            if (!result.IsValid)
                return BadRequest(result.Messages.Select(x => x.Message).ToList());

            return Ok(result.Data.Select(x => x.Path).ToList());
        }
    }
}
