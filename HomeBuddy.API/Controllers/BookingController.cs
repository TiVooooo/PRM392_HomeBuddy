using HomeBuddy.Service.Model.RequestDTO;
using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookingService.GetAllBookings());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _bookingService.GetBookingByID(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromForm] UpdateBookingRequest model)
        {
            return Ok(await _bookingService.UpdateBooking(id, model));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateNewBookingRequest model)
        {
            return Ok(await _bookingService.CreateNewBooking(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            return Ok(await _bookingService.DeleteBooking(id));
        }
    }
}
