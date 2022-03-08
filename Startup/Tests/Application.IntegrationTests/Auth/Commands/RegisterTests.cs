using Application.Auth.Commands.Register;
using Application.Exceptions;
using Domain.Entities.Identity;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Auth.Commands
{
    using static Testing;

    public class RegisterTests : TestBase
    {
        [Test]
        public async Task ShouldRegisterUser()
        {
            await EnsureSeedAsync();

            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "pero@gmail.com",
                Username = "preo",
                Password = "Pero_123!"
            };

            MediatR.Unit result = await SendAsync(command);

            int count = await CountAsync<AppUser>();

            result.Should().Be(MediatR.Unit.Value);
            count.Should().Be(2);
        }

        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "",
                Email = "",
                Username = "",
                Password = ""
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        #region [Test Email validation]

        [Test]
        public async Task ShouldRequireSpecificEmailFormat()
        {
            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perogmail.com",
                Username = "preo",
                Password = "Pero_123!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldRequireUniqueEmail()
        {
            await EnsureSeedAsync();

            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "admin@gmail.com",
                Username = "preo",
                Password = "Pero_123!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        #endregion [Test Email validation]

        #region [Test Password validation]

        [Test]
        public async Task ShouldRequirePasswordMinLenght()
        {
            await EnsureSeedAsync();

            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "preo",
                Password = "Pe_1!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldRequirePasswordToContainsLower()
        {
            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "preo",
                Password = "PERO_123!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldRequirePasswordToContainsNumbers()
        {
            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "preo",
                Password = "Pero_!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldRequirePasswordToContainsSpecialCharacters()
        {
            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "preo",
                Password = "Pero123"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldRequirePasswordToContainsUpper()
        {
            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "preo",
                Password = "pero_123!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        #endregion [Test Password validation]

        #region [Test Username validation]

        [Test]
        public async Task ShouldRequireUniqueUsername()
        {
            await EnsureSeedAsync();

            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "admin",
                Password = "Pero_123!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task ShouldRequireUsernameNotToContainsSpecialCharacters()
        {
            await EnsureSeedAsync();

            RegisterCommand command = new RegisterCommand()
            {
                ClientUri = "http://localhost:4200//confirm-email",
                Email = "perog@mail.com",
                Username = "preo&",
                Password = "Pero_123!"
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }

        #endregion [Test Username validation]
    }
}