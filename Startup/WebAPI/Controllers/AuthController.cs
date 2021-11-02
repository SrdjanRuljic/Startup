using Application.Auth.Queries.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class AuthController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginQuery query)
        {
            object token = await Mediator.Send(query);

            return Ok(token);
        }
    }
}
