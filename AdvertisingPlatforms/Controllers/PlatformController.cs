using AdvertisingPlatforms.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers;

[ApiController]
[Route("api/platforms")]
public class PlatformController(PlatformService platformService) : ControllerBase
{
    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл пустой");

        await using var stream = file.OpenReadStream();
        await platformService.UploadFile(stream);

        return Ok(new { message = "Файл загружен" });
    }
    
    [HttpGet("locations/{location}")]
    public IActionResult GetPlatformsByLocation(string location)
    {
        var platforms = platformService.GetPlatformsByRegion(location);
        return Ok(platforms);
    }
}