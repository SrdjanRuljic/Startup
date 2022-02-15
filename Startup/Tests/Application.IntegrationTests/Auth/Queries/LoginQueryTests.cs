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
            var query = new LoginQuery();

            FluentActions.Invoking(() =>
                SendAsync(query)).Should()
                                 .ThrowAsync<BadRequestException>();
        }
    }
}