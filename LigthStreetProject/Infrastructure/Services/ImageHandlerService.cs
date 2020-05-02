using Infrastructure.Helpers;
using Infrastructure.Services.Interfaces;
using LightStreet.Models.ImageModel;
using SkiaSharp;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public class ImageHandlerService : IImageHandlerService
    {
        private readonly IImageService _imageService;

        public ImageHandlerService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<IEnumerable<ImageModel>> Lightness(List<int> pointIds)
        {
            var list = new List<ImageModel>();
            for (int k = 0; k < pointIds.Count; k++)
            {
                int count = 0;
                var pixels = new List<Pixel>();
                SKBitmap bitmap = SKBitmap.Decode(await _imageService.DownloadIMageFromStorageAsync(pointIds[k].ToString()));
                Parallel.For(0, bitmap.Width, (i) =>
                {
                    Parallel.For(0, bitmap.Height, (j) =>
                    {
                        var rlin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Red));
                        var glin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Green));
                        var blin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Blue));
                        var Y = PixelInfo.CalculateYLuminence(rlin, glin, blin);
                        var lum = PixelInfo.CalculatePerceivedLightness(Y);
                        if (lum > PixelInfo.LIGHTNESS)
                        {
                            count++;
                        }
                    });
                    
                });
                
                var percents =  ((double)count / (bitmap.Width * bitmap.Height)) * 100;
                if(percents > 25)
                {
                    list.Add(new ImageModel() { PointId = pointIds[k], IsLight = true });
                }
                else
                {
                    list.Add(new ImageModel() { PointId = pointIds[k], IsLight = false });
                }
            }
            return list;
        }
    }
}
