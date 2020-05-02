using LightStreet.Models.ImageModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IImageHandlerService
    {
        public Task<IEnumerable<ImageModel>> Lightness(List<int> pointIds);
      
    }
}
