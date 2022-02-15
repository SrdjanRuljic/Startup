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
        public void ShouldRequireMinimumFields()
        {
            RegisterCommand command = new RegisterCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }
    }
}