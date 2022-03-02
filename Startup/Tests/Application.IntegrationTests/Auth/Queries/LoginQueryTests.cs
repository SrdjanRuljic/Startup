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

        #region [Test Password validation]

        [Test]
        public void ShouldRequirePasswordMinLenght()
        {
            LoginQuery query = new LoginQuery()
            {
                Username = "preo",
                Password = "Pero_1!"
            };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        [Test]
        public void ShouldRequirePasswordToContainsLower()
        {
            LoginQuery query = new LoginQuery()
            {
                Username = "preo",
                Password = "PERO_123!"
            };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        [Test]
        public void ShouldRequirePasswordToContainsNumbers()
        {
            LoginQuery query = new LoginQuery()
            {
                Username = "preo",
                Password = "Pero_!"
            };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        [Test]
        public void ShouldRequirePasswordToContainsspecialCharacters()
        {
            LoginQuery query = new LoginQuery()
            {
                Username = "preo",
                Password = "Pero123"
            };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        [Test]
        public void ShouldRequirePasswordToContainsUpper()
        {
            LoginQuery query = new LoginQuery()
            {
                Username = "preo",
                Password = "pero_123!"
            };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }

        #endregion [Test Password validation]

        [Test]
        public async Task ShouldReturnToken()
        {
        }
    }
}