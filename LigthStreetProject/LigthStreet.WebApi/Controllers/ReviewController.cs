using System.Threading.Tasks;
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System.Collections.Generic;
using LightStreet.WebApi.Models.Point;
using LightStreet.WebApi.Models.User;
using LightStreet.WebApi.Models.Review;
using Infrastructure.Models;
using LightStreet.WebApi.Models.Common;
using System.Linq;

namespace LigthStreet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewController(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPointsAndUser()
        {
            var listOfPoints = _mapper.Map<List<ViewPointModel>>(_mapper.Map<List<Point>>(await _unitOfWork.PointRepository.ToListAsync()));
            var listOfUsers = _mapper.Map<List<ViewUserModel>>(await _unitOfWork.UserRepository.GetAllActiveUsers());
            return Ok(new PointsWithUsersModel() { Points = listOfPoints, Users = listOfUsers }) ;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewModel model)
        {
            var review = _mapper.Map<Review>(model);
            review.CreatedById = 1;
            var reviewToSave = _mapper.Map<ReviewEntity>(review);
            await _unitOfWork.ReviewRepository.AddAsync(reviewToSave);
            await _unitOfWork.Commit();
            return Ok();
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetPendingUserPageAsync(int count = 10, int page = 0,
            string sEcho = null, string searchQuery = null)
        {
            var result = await _unitOfWork.ReviewRepository.GetPagesAsync(count, page, searchQuery);           
            return Ok(new ResponsePageResultModel<ViewReviewModel>(_mapper.Map<List<ViewReviewModel>>(result),
                sEcho, result.Count(), result.Count()));
        }
    }
}