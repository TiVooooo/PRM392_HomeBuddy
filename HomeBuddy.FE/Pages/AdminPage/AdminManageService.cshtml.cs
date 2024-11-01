using HomeBuddy.Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeBuddy.FE.Pages.AdminPage
{
    public class AdminManageServiceModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        public List<ServiceModel> Services { get; set; }
        public AdminManageServiceModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<IActionResult> OnPostAddServiceAsync(string Name, string Description, double Price, int Duration, int HelperId)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var newService = new Data.Models.Service
            {
                Name = Name,
                Description = Description,
                Price = Price,
                Duration = Duration,
                HelperId = HelperId
            };

            var response = await client.PostAsJsonAsync($"{baseUrl}/api/Service", newService);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            // Xử lý lỗi nếu không thêm được
            ModelState.AddModelError("", "Error adding service.");
            return Page();
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/api/Service");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<ServiceModel>>();
                if (result != null)
                {
                    Services = result;
                }
                else
                {
                    Services = new List<ServiceModel>();
                }
            }
            else
            {
                Services = new List<ServiceModel>();
            }
        }
    }
}
