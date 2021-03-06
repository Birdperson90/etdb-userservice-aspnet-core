using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.UserService.Domain.Entities;
using Etdb.UserService.Domain.ValueObjects;
using Etdb.UserService.Misc.Constants;
using Etdb.UserService.Repositories;
using MongoDB.Driver;

namespace Etdb.UserService.Scaffolder.Migrations
{
    public class DefaultDataFactory
    {
        private readonly UserServiceDbContext context;
        private readonly IHasher hasher;

        public DefaultDataFactory(UserServiceDbContext context, IHasher hasher)
        {
            this.context = context;
            this.hasher = hasher;
        }

        public async Task CreateDefaultDataAsync()
        {
            var roles = await this.CreateRolesAsync();
            await this.CreateUsersAsync(roles);
        }

        private async Task CreateUsersAsync(IEnumerable<SecurityRole> roles)
        {
            var usersCollection = this.context.Database.GetCollection<User>($"{nameof(User).ToLower()}s");

            var adminUser =
                (await usersCollection.FindAsync(user => user.UserName == "admin")).FirstOrDefault();

            if (adminUser != null) return;

            var salt = this.hasher.GenerateSalt();

            var adminGuid = Guid.NewGuid();

            var hashedPassword = new HashedPassword(this.hasher.CreateSaltedHash("12345678", salt), salt);

            adminUser = User.Create(adminGuid, "admin", null, null, null,
                DateTime.UtcNow,
                roles.Select(role => role.Id).ToArray(),
                new List<Email> {Email.Create(Guid.NewGuid(), "admin@etdb.com", true)},
                hashedPassword: hashedPassword);

            await usersCollection.InsertOneAsync(adminUser);
        }

        private async Task<IEnumerable<SecurityRole>> CreateRolesAsync()
        {
            var securityRoleCollection = this.context.Database.GetCollection<SecurityRole>("securityroles");

            var rolesToAdd = new List<SecurityRole>();

            var memberRole = (await securityRoleCollection.FindAsync(role => role.Name == RoleNames.Member))
                .FirstOrDefault();

            if (memberRole == null)
            {
                memberRole = new SecurityRole(Guid.NewGuid(), RoleNames.Member);

                rolesToAdd.Add(memberRole);
            }

            var adminRole = (await securityRoleCollection.FindAsync(role => role.Name == RoleNames.Admin))
                .FirstOrDefault();

            if (adminRole == null)
            {
                adminRole = new SecurityRole(Guid.NewGuid(), RoleNames.Admin);

                rolesToAdd.Add(adminRole);
            }

            if (rolesToAdd.Any())
            {
                await securityRoleCollection.InsertManyAsync(rolesToAdd);
            }

            return rolesToAdd;
        }
    }
}