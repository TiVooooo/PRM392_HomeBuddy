using HomeBuddy.Service.Model.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeBuddy.FE.Pages.AdminPage
{
    public class AdminManageBookingModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        public List<BookingResponseDTO> Bookings { get; set; }
        public AdminManageBookingModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/Booking");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BookingResponseDTO>>();
                if (result != null)
                {
                    Bookings = result;
                }
                else
                {
                    Bookings = new List<BookingResponseDTO>();
                }
            }
            else
            {
                Bookings = new List<BookingResponseDTO>();
            }
        }
    }
}