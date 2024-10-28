using FirebaseAdmin.Messaging;
using HomeBuddy.Common;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model.RequestDTO;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var loginUser = await _loginService.Login(loginRequest);
            return Ok(loginUser);
        }
    }
}
