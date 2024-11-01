using HomeBuddy.Data.Models;
using HomeBuddy.Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeBuddy.FE.Pages.AdminPage
{
    public class UserManagementModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? newRole { get; set; }
        public List<User> Users { get; set; }
        public UserManagementModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/api/User");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                Users = result?.Data ?? new List<User>();
            }
            else
            {
                Users = new List<User>();
            }
        }
        public async Task<IActionResult> OnPostChangeRoleAsync(int? UserId)
        {
            if (UserId == null || (newRole != "User" && newRole != "Manager" && newRole != "Helper"))
            {
                ModelState.AddModelError("", "Invalid user or role.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var apiUrl = $"{baseUrl}/api/User/change-role?id={UserId.Value}&newRole={newRole}";

            try
            {
                var response = await client.PutAsync(apiUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Role updated successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Unable to change role.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Request failed: {ex.Message}");
            }

            await OnGetAsync(); // Refresh user list
            return RedirectToPage("/AdminPage/UserManagement");
        }

        public class UserResponse
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<User> Data { get; set; }
        }
    }
}