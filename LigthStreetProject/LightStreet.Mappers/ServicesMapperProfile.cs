using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Domain.Models.ManyToMany;
using Infrastructure.Models;
using Infrastructure.Models.Enums;
using Infrastructure.Models.ManyToMany;
using System;

namespace LightStreet.Mappers
{
    public class ServicesMapperProfile : Profile
    {
        public ServicesMapperProfile()
        {
            #region Domain => Entity

            CreateMap<PendingType, PendingTypeEntity>();
            CreateMap<Tag, TagEntity>();
            CreateMap<UserTag, UserTagEntity>();
            CreateMap<PendingUser, PendingUserEntity>();
            CreateMap<User, UserEntity>();
            CreateMap<UserRole, UserRoleEntity>();
            CreateMap<Role, RoleEntity>();
            CreateMap<RolePermission, RolePermissionEntity>();
            CreateMap<Permission, PermissionEntity>();
            CreateMap<UserStatusType, UserStatusTypeEntity>();

            #endregion Domain => Entity

            #region Entity => Domain

            CreateMap<PendingTypeEntity, PendingType>();
            CreateMap<TagEntity, Tag>();
            CreateMap<UserTagEntity, UserTag>();
            CreateMap<PendingUserEntity, PendingUser>();
            CreateMap<UserEntity, User>();
            CreateMap<UserRoleEntity, UserRole>();
            CreateMap<RoleEntity, Role>();
            CreateMap<RolePermissionEntity, RolePermission>();
            CreateMap<PermissionEntity, Permission>();

            #endregion Entity => Domain

            #region Domain => Domain

            CreateMap<PendingUser, User>()
                .ForMember(x => x.Status, w => w.Ignore())
                .ForMember(x => x.Id, w => w.Ignore());
            CreateMap<User, PendingUser>()
                .ForMember(x => x.Status, w => w.Ignore());

            #endregion Domain => Domain
        }
    }
}
