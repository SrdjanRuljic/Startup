using Application.Auth.Queries.Login;
using Application.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            LoginQuery query = new LoginQuery()
            {
                Username = "admin",
                Password = "Administrator_123!"
            };

            object result = await SendAsync(query);

            result.Should()
                  .NotBeNull();
        }
    }
}