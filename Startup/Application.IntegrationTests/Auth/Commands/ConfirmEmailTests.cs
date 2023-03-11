using Application.Auth.Commands.ConfirmEmail;
using Application.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Auth.Commands
{
    using static Testing;

    public class ConfirmEmailTests : BaseTestFixture
    {
        [Test]
        public async Task ShouldRequireExistingUser()
        {
            ConfirmEmailCommand command = new ConfirmEmailCommand()
            {
                UserName = "marinko",
                Token = "Test",
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            ConfirmEmailCommand command = new ConfirmEmailCommand()
            {
                UserName = string.Empty,
                Token = string.Empty,
            };

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should()
                                   .ThrowAsync<BadRequestException>();
        }
    }
}