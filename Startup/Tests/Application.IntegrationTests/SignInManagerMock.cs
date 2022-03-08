using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Application.IntegrationTests
{
    public class SignInManagerMock : SignInManager<AppUser>
    {
        public SignInManagerMock(UserManager<AppUser> userManager,
                                 IHttpContextAccessor contextAccessor,
                                 IUserClaimsPrincipalFactory<AppUser> claimsFactory,
                                 IOptions<IdentityOptions> optionsAccessor,
                                 ILogger<SignInManager<AppUser>> logger,
                                 IAuthenticationSchemeProvider schemes,
                                 IUserConfirmation<AppUser> confirmation) : base(userManager,
                                                                                 contextAccessor,
                                                                                 claimsFactory,
                                                                                 optionsAccessor,
                                                                                 logger,
                                                                                 schemes,
                                                                                 confirmation)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName,
                                                               string password,
                                                               bool isPersistent,
                                                               bool lockoutOnFailure)
        {
            if (userName == "admin" && password == "Administrator_123!")
            {
                return Task.FromResult(SignInResult.Success);
            }

            return Task.FromResult(SignInResult.Failed);
        }
    }
}