﻿using System.Threading.Tasks;
using Etdb.UserService.Controllers.Tests.Fixtures;
using Xunit;

namespace Etdb.UserService.Controllers.Tests
{
    public class DummyControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture testServerFixture;

        public DummyControllerTests(TestServerFixture testServerFixture)
        {
            this.testServerFixture = testServerFixture;
        }

        [Fact]
        public async Task TestDummy()
        {
            var client = this.testServerFixture.ApiServer.CreateClient();

            var response = await client.GetAsync("api/v1/dummy");

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}