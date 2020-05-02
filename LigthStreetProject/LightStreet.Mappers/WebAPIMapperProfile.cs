using AutoMapper;
using Domain.Models;
using LightStreet.WebAPI.Models.PendingUser;
using System;
using System.Linq;

namespace LightStreet.Mappers
{
    public class WebApiMapperProfile : Profile
    {
        public WebApiMapperProfile()
        {
            CreateMap<PendingUser, ViewPendingUserModel>();
            #region API => Entity

            CreateMap<SingUpPendingUserModel, PendingUser>()
                .ForMember(x => x.ModifiedAt, w => w.MapFrom(q => DateTimeOffset.Now))
                .ForMember(x => x.UserName, q => q.MapFrom(w => w.Login));

            #endregion API => Entity
        }
    }
}