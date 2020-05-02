using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using Infrastructure;
using LightStreet.WebAPI.Models.Common;
using LightStreet.WebAPI.Models.PendingUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LigthStreet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingUserPageAsync(int count = 10, int page = 0,
           string sEcho = null, string searchQuery = null,
           PendingType pendingType = PendingType.Pending)
        {
            var result =
                await _unitOfWork.PendingUserRepository.GetPagesAsync(count, page, searchQuery);
            return Ok(new ResponsePageResultModel<ViewPendingUserModel>(_mapper.Map<List<ViewPendingUserModel>>(result),
                sEcho, result.Count(), result.Count()));
        }
    }
}