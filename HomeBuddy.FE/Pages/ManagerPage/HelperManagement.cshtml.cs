using HomeBuddy.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeBuddy.FE.Pages.ManagerPage
{
    public class HelperManagerMentModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        public List<Helper> Helpers { get; set; }
        public HelperManagerMentModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/Helper");

            if (response.IsSuccessStatusCode)
            {
                Helpers = await response.Content.ReadFromJsonAsync<List<Helper>>() ?? new List<Helper>();
            }
            else
            {
                Helpers = new List<Helper>(); 
            }
        }
    }
}
