using Infrastructure.Helpers;
using Infrastructure.Services.Interfaces;
using LightStreet.WebApi.Models.ImageModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Extensions.Configuration;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace Infrastructure.Services
{
    public class ImageHandlerService : IImageHandlerService
    {
        private readonly IImageService _imageService;
        private readonly IConfiguration _config;

        public ImageHandlerService(IImageService imageService, IConfiguration config)
        {
            _imageService = imageService;

            _config = config;
        }

        public string GetLightnessPixelsAsync(IFormFile file,int lightness)
        {
            SKBitmap bitmap = SKBitmap.Decode(file.OpenReadStream());

            for (int i = 0; i < bitmap.Width; ++i)
            {
                for (int j = 0; j < bitmap.Height; ++j)
                {
                    var rlin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Red));
                    var glin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Green));
                    var blin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Blue));

                    var Y = PixelInfo.CalculateYLuminence(rlin, glin, blin);

                    var lum = PixelInfo.CalculatePerceivedLightness(Y);
                    if (lum > lightness)
                    {
                        bitmap.SetPixel(i, j, new SKColor(103, 235, 52));
                    }
                }
            }

            var image = SKImage.FromBitmap(bitmap);

            var imageStream = image.Encode().AsStream();

            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);

                var bytesArray = ms.ToArray();

                return Convert.ToBase64String(bytesArray);
            }

        }

        public string SortPixelsAsync(IFormFile file)
        {
            SKBitmap bitmap = SKBitmap.Decode(file.OpenReadStream());

            var pixels = new List<Pixel>();

            for(int i = 0; i < bitmap.Width; ++i)
            {
                for(int j = 0; j < bitmap.Height; ++j)
                {         
                    var rlin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Red));
                    var glin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Green));
                    var blin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Blue));

                    var Y = PixelInfo.CalculateYLuminence(rlin, glin, blin);

                    pixels.Add(new Pixel()
                    {
                        Red = bitmap.GetPixel(i, j).Red,
                        Green = bitmap.GetPixel(i, j).Green,
                        Blue = bitmap.GetPixel(i, j).Blue,
                        Luminence = PixelInfo.CalculatePerceivedLightness(Y)
                    });
                }
            }

            pixels.Sort();

            int k = 0;
            for (int i = 0; i < bitmap.Width; ++i)
            {
                for (int j = 0; j < bitmap.Height; ++j)
                {
                    bitmap.SetPixel(i, j, new SKColor(pixels[k].Red, pixels[k].Green, pixels[k].Blue));

                    k++;
                }
            }

            var image = SKImage.FromBitmap(bitmap);

            var imageStream = image.Encode().AsStream();

            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);

                var bytesArray = ms.ToArray();

                return Convert.ToBase64String(bytesArray);
            }
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

        public async Task<string> DetectAsync(IFormFile file, int lightness)
        {
            var predictionSection = _config.GetSection("PredictionApi");

            var client = new CustomVisionPredictionClient
            {
                ApiKey = predictionSection["key"],
                Endpoint = predictionSection["endpoint"]
            };

            var result = await client.DetectImageAsync(Guid.Parse(predictionSection["projectid"]),
                predictionSection["iteration"], file.OpenReadStream());

            if (lightness == -1)
            {
                return DrawRectangles(file, result);
            }
            else
            {
                return DrawLightRectangles(file, result, lightness);
            }
        }

        public string DrawLightRectangles(IFormFile file, ImagePrediction predictionResult, int lightness)
        {
            SKBitmap bitmap = SKBitmap.Decode(file.OpenReadStream());

            foreach (var prediction in predictionResult.Predictions)
            {
                if (prediction.Probability > 0.2)
                {
                    int x = (int)(prediction.BoundingBox.Left * bitmap.Width + 0.5);
                    int y = (int)(prediction.BoundingBox.Top * bitmap.Height + 0.5);

                    int width = (int)(prediction.BoundingBox.Width * bitmap.Width + 0.5);
                    int height = (int)(prediction.BoundingBox.Height * bitmap.Height + 0.5);

                    for (int i = x; i < x + width; ++i)
                    {
                        for (int j = y; j < y + height; ++j)
                        {
                            var rlin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Red));
                            var glin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Green));
                            var blin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(bitmap.GetPixel(i, j).Blue));

                            var Y = PixelInfo.CalculateYLuminence(rlin, glin, blin);

                            var lum = PixelInfo.CalculatePerceivedLightness(Y);
                            if (lum > lightness)
                            {
                                bitmap.SetPixel(i, j, new SKColor(103, 235, 52));
                            }
                        }
                    }

                }
            }

            var image = SKImage.FromBitmap(bitmap);

            var imageStream = image.Encode().AsStream();

            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);

                var bytesArray = ms.ToArray();

                return Convert.ToBase64String(bytesArray);
            }
        }

        public string DrawRectangles(IFormFile file, ImagePrediction predictionResult)
        {
            SKBitmap bitmap = SKBitmap.Decode(file.OpenReadStream());

            SKCanvas canvas = new SKCanvas(bitmap);

            SKPaint paint = new SKPaint
            {
                IsAntialias = true,
                Color = new SKColor(220, 38, 38),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 3
            };


            foreach (var prediction in predictionResult.Predictions)
            {
                if (prediction.Probability > 0.2)
                {
                    var x = prediction.BoundingBox.Left * bitmap.Width;
                    var y = prediction.BoundingBox.Top * bitmap.Height;

                    var width = prediction.BoundingBox.Width * bitmap.Width;
                    var height = prediction.BoundingBox.Height * bitmap.Height;

                    canvas.DrawRect((float)x, (float)y, (float)width, (float)height, paint);
                }
            }

            var image = SKImage.FromBitmap(bitmap);

            var imageStream = image.Encode().AsStream();

            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);

                var bytesArray = ms.ToArray();

                return Convert.ToBase64String(bytesArray);
            }
        }
    }
}
