using LightStreet.Models.ImageModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IImageHandlerService
    {
        public Task<IEnumerable<ImageModel>> Lightness(List<int> pointIds);

        string SortPixelsAsync(IFormFile file);

        string GetLightnessPixelsAsync(IFormFile file, int lightness);
    }
}
