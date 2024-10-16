using HomeBuddy.Common;
using HomeBuddy.Service.Model;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _serviceService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDTO model)
        {
            var result = await _serviceService.Save(model);
            return Ok(result);
        }

    }
}
