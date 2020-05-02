using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infrastructure;
using Infrastructure.Models;
using LightStreet.WebAPI.Models.Common;
using LightStreet.WebAPI.Models.PendingUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        [AllowAnonymous]
        [HttpPost("pending/status")]
        public async Task<IActionResult> ChangePendingAgentStatus([FromBody] ChangePendingUserStatusModel model)
        {
            var pendingUser = await _unitOfWork.PendingUserRepository.FindByIdAsync(model.UserId);
            if (pendingUser != null)
            {
                var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                int currentUserId = 1;
                if (claim != null)
                {
                   currentUserId = int.Parse(claim.Value);
                }
                var approvedUser = await _unitOfWork.PendingUserRepository.ApproveUserAndSetTagsAsync(_mapper.Map<PendingUser>(pendingUser), model.Status, model.Tags, currentUserId);
                if (model.RoleId != 0)
                {
                    await _unitOfWork.UserRepository.ChangeUserRoleAsync(approvedUser.Id, model.RoleId);
                }
                return Ok();
            }
            throw new Exception("User not found");
        }
    }
}