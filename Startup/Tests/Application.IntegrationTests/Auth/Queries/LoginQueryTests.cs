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
        public void ShouldRequireMinimumFields()
        {
            LoginQuery query = new LoginQuery();

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldReturnToken()
        {
        }
    }
}