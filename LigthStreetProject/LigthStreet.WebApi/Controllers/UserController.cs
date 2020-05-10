using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infrastructure;
using Infrastructure.Models.Enums;
using Infrastructure.Repositories.Interfaces;
using LightStreet.WebApi.Models.Common;
using LightStreet.WebApi.Models.PendingUser;
using LightStreet.WebApi.Models.User;
using LightStreet.WebApi.Models.UserTags;
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

        [AllowAnonymous]
        [HttpPost("pending/status")]
        public async Task<IActionResult> ChangePendingAgentStatus([FromBody] ChangePendingUserStatusModel model)
        {
            var pendingUser = await _unitOfWork.PendingUserRepository.FindByIdAsync(model.UserId);
            if (pendingUser != null)
            {
                int currentUserId = 1;
                var approvedUser = await _unitOfWork.PendingUserRepository.ApproveUserAndSetTagsAsync(_mapper.Map<PendingUser>(pendingUser), model.Status, model.Tags, currentUserId);
                if (model.RoleId != 0)
                {
                    await _unitOfWork.UserRepository.ChangeUserRoleAsync(approvedUser.Id, model.RoleId);
                }
                return Ok();
            }
            throw new Exception("User not found");
        }

        [AllowAnonymous]
        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedAgentPageAsync(int count = 10, int page = 0,
           string sEcho = null, string searchQuery = null,
           UserStatusTypeEntity status = UserStatusTypeEntity.Active)
        {
            var result = await _unitOfWork.UserRepository.GetPageAsync(count, page, searchQuery, (UserStatusTypeEntity)status.GetHashCode());
            return Ok(new ResponsePageResultModel<ViewUserModel>(_mapper.Map<List<ViewUserModel>>(result),
                sEcho, result.Count(), result.Count()));
        }

        [HttpPost("approved/status")]
        public async Task<IActionResult> ChangeAgentStatus([FromBody] ChangeUserStatusModel model)
        {
            if (await _unitOfWork.UserRepository.UserExists(model.UserId))
            {
                await _unitOfWork.UserRepository.ChangeStatusUserAsync(model.UserId, model.Status);
                return Ok();
            }
            throw new Exception("Agent not found");
        }

        [HttpGet("pending/unAuthorize")]
        public async Task<IActionResult> UnAuthorizePendingAsync(int userId)
        {
            var pendingUserForDelete = await _unitOfWork.PendingUserRepository.FindByIdAsync(userId);
            _unitOfWork.PendingUserRepository.Delete(pendingUserForDelete);
            await _unitOfWork.Commit();
            return Ok();
        }

        [HttpGet("approved/unAuthorize")]
        public async Task<IActionResult> UnAuthorizeAsync(int userId)
        {
            var userForDelete = await _unitOfWork.UserRepository.FindByIdAsync(userId);
            userForDelete.IsDeleted = true;
            await _unitOfWork.Commit();
            return Ok();
        }

        [HttpPost("changerole")]
        public async Task<IActionResult> ChangeRoleAsync(ChangeUserRoleModel model)
        {
            await _unitOfWork.UserRepository.ChangeUserRoleAsync(model.UserId, model.RoleId);
            return Ok();
        }

        [HttpPost("tag")]
        public async Task<IActionResult> AddTagAsync([FromBody] ChangeTagsUserModel model)
        {
            if (model.AddedTagList != null)
            {
                foreach (var userTagBindingModel in model.AddedTagList)
                {
                    await _unitOfWork.TagRepository.AddUsersTagAsync(userTagBindingModel.Name, userTagBindingModel.UserIds);
                }
            }

            if (model.DeletedTagList != null)
            {
                foreach (var deleteUserTagModel in model.DeletedTagList)
                {
                    await _unitOfWork.TagRepository.DeleteUserTagsAsync(deleteUserTagModel.UserId, deleteUserTagModel.TagIds);
                }
            }

            return Ok();
        }
    }
}