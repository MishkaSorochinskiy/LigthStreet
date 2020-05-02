using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Infrastructure;
using LightStreet.WebAPI.Models.PendingUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LigthStreet.WebApi.Controllers
{
    [Route("api/users/authorize")]
    [ApiController]
    public class AuthorizeUserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorizeUserController(IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SingUpPendingUserModel model)
        {
            if(!await _unitOfWork.PendingUserRepository.EmailExists(model.Email))
            {
                if(!await _unitOfWork.PendingUserRepository.UsernameExists(model.Login))
                {
                    var user = _mapper.Map<PendingUser>(model);
                    user.ModifiedAt = DateTimeOffset.UtcNow;
                    var result = await _unitOfWork.PendingUserRepository.RegisterAsync(user, model.Password);
                    return Ok();
                }
                throw new Exception("User with same email already exist");
            }
            throw new Exception("User with same username already exist");
        }
    }
}
