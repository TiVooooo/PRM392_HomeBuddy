using HomeBuddy.Service.Model.ResponseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static HomeBuddy.FE.Pages.AdminPage.UserManagementModel;

namespace HomeBuddy.FE.Pages.AdminPage
{
    public class DashboardModel : PageModel
    {
        public DashboardResponse Dashboard { get; set; }
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public DashboardModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/Dashboard");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DashboardResponse>();
                if(result != null)
                {
                    Dashboard = result;
                }
            }
            else
            {
                Dashboard = new DashboardResponse();
            }
        }
    }
}
