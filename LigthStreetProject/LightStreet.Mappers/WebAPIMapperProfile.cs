using AutoMapper;
using Domain.Models;
using LightStreet.WebAPI.Models.PendingUser;
using LightStreet.WebAPI.Models.Tag;
using LightStreet.WebAPI.Models.User;
using System;
using System.Linq;

namespace LightStreet.Mappers
{
    public class WebApiMapperProfile : Profile
    {
        public WebApiMapperProfile()
        {
            CreateMap<Tag, ViewTagModel>();
            CreateMap<PendingUser, ViewPendingUserModel>();
            CreateMap<User, ViewUserModel>()
               .ForMember(x => x.Tags, q => q.MapFrom(w => w.UserTags.Select(r => r.Tag)))
               .ForMember(x => x.CreatedByUserName, w => w.MapFrom(q => q.ModifiedBy.UserName))
               .ForMember(x => x.RoleName, q => q.MapFrom(w => w.UserRoles.Select(r => r.Role.Name).FirstOrDefault()))
               .ForMember(x => x.RoleId, q => q.MapFrom(w => w.UserRoles.Select(r => r.Role.Id).FirstOrDefault()));

            #region API => Entity

            CreateMap<SingUpPendingUserModel, PendingUser>()
                .ForMember(x => x.ModifiedAt, w => w.MapFrom(q => DateTimeOffset.Now))
                .ForMember(x => x.UserName, q => q.MapFrom(w => w.Login));

            #endregion API => Entity
        }
    }
}