using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace HomeBuddy.API.Controllers
{
    [ApiController]
    [Route("Helper")]
    public class HelperController : ControllerBase
    {
        private readonly IHelperService _helpService;

        public HelperController(IHelperService helpService)
        {
            _helpService = helpService;
        }
        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _helpService.ChangeStatus(id);
            return StatusCode((int)result.Status, result.Data == null ? result.Message : result.Data);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _helpService.GetAll();
            return StatusCode((int)result.Status, result.Data == null ? result.Message : result.Data);
        }        
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _helpService.GetById(id);
            return StatusCode((int)result.Status, result.Data == null ? result.Message : result.Data);
        }
    }
}
