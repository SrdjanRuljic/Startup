using Application.Auth.Queries.Login;
using Application.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Auth.Queries
{
    using static Testing;

    public class LoginQueryTests : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            LoginQuery query = new LoginQuery()
            {
                Username = "",
                Password = ""
            };

            await FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldReturnToken()
        {
            await EnsureSeedAsync();

            LoginQuery query = new LoginQuery()
            {
                Username = "admin",
                Password = "Administrator_123!"
            };

            object result = await SendAsync(query);

            result.Should().NotBeNull();
        }
    }
}