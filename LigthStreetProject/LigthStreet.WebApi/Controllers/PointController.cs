using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Root;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Services.Interfaces;
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

        public PointController(IUnitOfWork unitOfWork,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        [HttpGet]
        [Route("points")]
        public async Task<IEnumerable<PointEntity>> GetAllPoints(double west,double east,double north,double south)
        {
            return await _unitOfWork.PointRepository.GetFromZone(west,east,north,south);
        }

        [HttpGet("{pointId}")]
        public async Task<string> GetByPointId(int pointId)
        {
            return await _imageService.DownloadIMageFromStorageAsync(pointId.ToString());
        }

        [HttpPost]
        [Route("point")]
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
    }
}