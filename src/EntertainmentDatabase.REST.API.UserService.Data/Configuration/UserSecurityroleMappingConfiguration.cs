﻿using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.UserService.Data.Configuration
{
    internal class UserSecurityroleMappingConfiguration : EntityMappingConfiguration<UserSecurityrole>
    {
        public UserSecurityroleMappingConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<UserSecurityrole> builder)
        {
            base.Configure(builder);

            builder.HasOne(userSecurityrole => userSecurityrole.User)
                .WithMany(user => user.UserSecurityroles);

            builder.HasOne(userSecurityrole => userSecurityrole.Securityrole)
                .WithMany(role => role.UserSecurityroles);
        }
    }
}
