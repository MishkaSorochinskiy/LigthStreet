using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LigthStreet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoImageController : ControllerBase
    {
        private IImageHandlerService _imageHandlerService;

        public DemoImageController(IImageHandlerService imageHandlerService)
        {
            _imageHandlerService = imageHandlerService;
        }

        [HttpPost]
        [Route("sortpixels")]
        public IActionResult SortPixels(IFormFile file)
        {
            var photo = _imageHandlerService.SortPixelsAsync(file);

            return Ok(photo);
        }

        [HttpPost]
        [Route("lightpixels")]
        public IActionResult GetLightPixels(IFormFile file,[FromQuery]int lightness)
        {
            var photo = _imageHandlerService.GetLightnessPixelsAsync(file,lightness);

            return Ok(photo);
        }

        [HttpPost]
        [Route("Detect")]
        public async Task<IActionResult> GetDetected(IFormFile file)
        {
            var photo = await _imageHandlerService.DetectAsync(file);

            return Ok(photo);
        }
    }
}