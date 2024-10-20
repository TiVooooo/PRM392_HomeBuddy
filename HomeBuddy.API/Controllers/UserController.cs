using HomeBuddy.Service.Model;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO request)
        {
            var result = await _userService.Create(request);

            return Ok(result);
        }
        [HttpPost("create-helper")]
        public async Task<IActionResult> CreateHelper([FromBody] CreateHelperDTO request)
        {
            var result = await _userService.CreateHelper(request);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO request)
        {
            var result = await _userService.Update(id, request);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);

            return Ok(result);
        }
        [HttpPut("edit-avatar/{id}")]
        public async Task<IActionResult> EditAvatar(int id, IFormFile avatar)
        {
            var result = await _userService.EditAvatar(id, avatar);

            return Ok(result);
        }
    }
}
