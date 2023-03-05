using Application.Auth.Commands.Login;
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
            LoginCommand query = new LoginCommand()
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

            LoginCommand query = new LoginCommand()
            {
                Username = "admin",
                Password = "Administrator_123!"
            };

            object result = await SendAsync(query);

            result.Should().NotBeNull();
        }
    }
}