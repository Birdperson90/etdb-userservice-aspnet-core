﻿using AutoMapper;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.EntityInfos;
using Etdb.UserService.Presentation.DataTransferObjects;

namespace Etdb.UserService.Application.Automapper.Profiles
{
    public class SecurityRoleProfile : Profile
    {
        public SecurityRoleProfile()
        {
            this.CreateMap<SecurityRole, SecurityRoleDto>()
                .ReverseMap();

            this.CreateMap<SecurityRoleInfo, SecurityRoleInfoDto>()
                .ReverseMap();
        }
    }
}