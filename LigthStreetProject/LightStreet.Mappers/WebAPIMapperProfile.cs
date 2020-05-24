using AutoMapper;
using Domain.Models;
using LightStreet.WebApi.Models.PendingUser;
using LightStreet.WebApi.Models.Point;
using LightStreet.WebApi.Models.Review;
using LightStreet.WebApi.Models.Tag;
using LightStreet.WebApi.Models.User;
using System;
using System.Linq;

namespace LightStreet.Mappers
{
    public class WebApiMapperProfile : Profile
    {
        public WebApiMapperProfile()
        {
            CreateMap<Tag, ViewTagModel>();
            CreateMap<Point, ViewPointModel>();
            CreateMap<PendingUser, ViewPendingUserModel>();
            CreateMap<User, ViewUserModel>()
               .ForMember(x => x.Tags, q => q.MapFrom(w => w.UserTags.Select(r => r.Tag)))
               .ForMember(x => x.CreatedByUserName, w => w.MapFrom(q => q.ModifiedBy.UserName))
               .ForMember(x => x.RoleName, q => q.MapFrom(w => w.UserRoles.Select(r => r.Role.Name).FirstOrDefault()))
               .ForMember(x => x.RoleId, q => q.MapFrom(w => w.UserRoles.Select(r => r.Role.Id).FirstOrDefault()));
            CreateMap<Review, ViewReviewModel>()
               .ForMember(x => x.CreatedBy, w => w.MapFrom(q => q.CreatedBy.UserName))
               .ForMember(x => x.ApplyOn, q => q.MapFrom(w => w.ApplyOn.UserName));

            #region API => Entity

            CreateMap<SingUpPendingUserModel, PendingUser>()
                .ForMember(x => x.ModifiedAt, w => w.MapFrom(q => DateTimeOffset.Now))
                .ForMember(x => x.UserName, q => q.MapFrom(w => w.Login));

            CreateMap<AddReviewModel, Review>();              
            #endregion API => Entity
        }
    }
}