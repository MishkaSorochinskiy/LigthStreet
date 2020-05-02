using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Root;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Services.Interfaces;
using LightStreet.Models.ImageModel;
using LightStreet.Models.PointModel;
using Microsoft.AspNetCore.Mvc;

namespace LigthStreet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IImageService _imageService;

        private readonly IImageHandlerService _imageHandlerService;

        public PointController(IUnitOfWork unitOfWork,
            IImageService imageService,
            IImageHandlerService imageHandlerService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _imageHandlerService = imageHandlerService;
        }

        [HttpGet]
        [Route("points")]
        public async Task<IEnumerable<PointEntity>> GetAllPoints(double west,double east,double north,double south)
        {
            return await _unitOfWork.PointRepository.GetFromZone(west,east,north,south);
        }

        [HttpGet("lightpoints")]
        public async Task<IEnumerable<ImageModel>> GetByPointId(List<int> pointIds)
        {
            return await _imageHandlerService.Lightness(pointIds);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddPointAsync(AddPoint point)
        {
            var existingPoint = await _unitOfWork.PointRepository.GetByCoords(point.Latitude, point.Longtitude);

            if (existingPoint == null)
            {
                existingPoint = new PointEntity() { Latitude = point.Latitude, Longtitude = point.Longtitude };
                
                await _unitOfWork.PointRepository.AddAsync(existingPoint);
                
                await _unitOfWork.Commit();
            }

            await _imageService.UploadImageToStorageAsync(existingPoint.Id.ToString(), point.Image);
            
            return Ok(existingPoint.Id);
        }

        [HttpPost]
        [Route("lightness")]
        public async Task<IActionResult> GetLightness ([FromBody]List<int> pointsId)
        {
            var from = DateTime.Now;

            var lightnessData = await _imageHandlerService.Lightness(pointsId);

            var to = DateTime.Now;

            var dif = to.Subtract(from);

            var sec = dif.TotalMilliseconds;

            return Ok(lightnessData);
        }
    }
}