using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Root;
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
        public async Task<IEnumerable<Point>> GetAllPoints()
        {
            return await _unitOfWork.PointRepository.ToListAsync();
        }

        [HttpGet("{pointId}")]
        public async Task<string> GetByPointId(int pointId)
        {
            return await _imageService.DownloadIMageFromStorageAsync(pointId.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> AddPointAsync([FromBody] AddPoint point)
        {
            var newEntity = new Point(point.Latitude, point.Longtitude);
            await _unitOfWork.PointRepository.AddAsync(newEntity);
            await _unitOfWork.Commit();
            await _imageService.UploadImageToStorageAsync(newEntity.Id.ToString(), point.Image);
            return Ok(newEntity.Id);
        }
    }
}