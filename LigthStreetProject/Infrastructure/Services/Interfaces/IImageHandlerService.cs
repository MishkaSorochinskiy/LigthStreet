using LightStreet.WebApi.Models.ImageModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces
{
    public interface IImageHandlerService
    {
        public Task<IEnumerable<ImageModel>> Lightness(List<int> pointIds);

        string SortPixelsAsync(IFormFile file);

        string GetLightnessPixelsAsync(IFormFile file, int lightness);
    }
}
