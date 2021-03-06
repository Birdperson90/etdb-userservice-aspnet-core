using System;
using Autofac;
using Elders.RedLock;
using Etdb.ServiceBase.Cryptography.Abstractions.Hashing;
using Etdb.ServiceBase.Services.Abstractions;
using Etdb.UserService.Authentication.Abstractions.Strategies;
using Etdb.UserService.Autofac.Extensions;
using Etdb.UserService.Domain.Enums;
using Etdb.UserService.Repositories;
using Etdb.UserService.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Etdb.UserService.Bootstrap.Tests
{
    public class ContainerBuilderExtensionsIntegrationTests
    {
        [Fact]
        public void ContainerBuilderExtensions_SetupDependencies_Dependencies_Registered_And_Can_Be_Resolved()
        {
            var containerBuilder = new ContainerBuilder();

            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            webHostEnvironmentMock.Object.EnvironmentName = Environments.Development;

            containerBuilder.SetupDependencies(webHostEnvironmentMock.Object);

            var container = containerBuilder.Build();

            Assert.True(container.IsRegistered<IRedisLockManager>(), $"{nameof(IRedisLockManager)} not registered");
            Assert.True(container.IsRegistered<IUserUrlFactory>(),
                $"{nameof(IUserUrlFactory)} not registered");
            Assert.True(container.IsRegistered<IHasher>(), $"{nameof(IHasher)} not registered");
            Assert.True(container.IsRegistered<IFileService>(), $"{nameof(IFileService)} not registered");
            Assert.True(container.IsRegistered<UserServiceDbContext>(),
                $"{nameof(UserServiceDbContext)} not registered");
            Assert.True(container.IsRegistered<IMediator>(), $"{nameof(IMediator)} not registered");
            Assert.True(container.IsRegistered<IHttpContextAccessor>(),
                $"{nameof(IHttpContextAccessor)} not registered");
            Assert.True(container.IsRegistered<IGoogleAuthenticationStrategy>(),
                $"{nameof(IGoogleAuthenticationStrategy)} not registered");
            Assert.True(container.IsRegistered<IFacebookAuthenticationStrategy>(),
                $"{nameof(IFacebookAuthenticationStrategy)} not registered");
            Assert.True(container.IsRegistered<IUsersService>(), $"{nameof(IUsersService)} not registered");
            Assert.True(container.IsRegistered<IResourceLockingAdapter>(),
                $"{nameof(IResourceLockingAdapter)} not registered");
            Assert.True(container.IsRegistered<Func<AuthenticationProvider, IExternalAuthenticationStrategy>>(),
                $"{nameof(Func<AuthenticationProvider, IExternalAuthenticationStrategy>)} not registered");
        }
    }
}