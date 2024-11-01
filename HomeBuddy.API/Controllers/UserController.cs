using HomeBuddy.Service.Model;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

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
        [HttpPut("parentId")]
        public async Task<IActionResult> UpdateParentId(int userId, int parentId)
        {
            var result = await _userService.UpdateParentId(userId, parentId);
            return StatusCode((int)result.Status, result.Data == null ? result.Message : result.Data);
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

        [HttpPut("change-role")]
        public async Task<IActionResult> ChangeRole(int id, string newRole)
        {
            var result = await _userService.ChangeRole(id, newRole);

            return Ok(result);
        }
        [HttpGet("manager")]
        public async Task<IActionResult> GetAllManager()
        {
            var result = await _userService.GetAllManager();

            return Ok(result);
        }
    }
}
