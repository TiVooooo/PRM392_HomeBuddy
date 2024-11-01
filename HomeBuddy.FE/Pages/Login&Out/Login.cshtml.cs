using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;

namespace HomeBuddy.FE.Pages.Login_Out
{

    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var loginUrl = $"{baseUrl}/api/Login";

            _logger.LogInformation($"Attempting to login at URL: {loginUrl}");

            try
            {
                var response = await client.PostAsJsonAsync(loginUrl, LoginRequest);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Response status code: {response.StatusCode}");
                _logger.LogInformation($"Response content: {content}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<LoginApiResponse>(content, options);

                if (apiResponse?.Data != null)
                {
                    var loginResult = apiResponse.Data; 

                    HttpContext.Session.SetString("JWTToken", loginResult.Token);
                    HttpContext.Session.SetString("UserRole", loginResult.Role);
                    HttpContext.Session.SetString("TokenExpiration", loginResult.Expiration.ToString());

                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(loginResult.Token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                    var userName = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                    var userId = loginResult.UserId.ToString(); 

                    if (!string.IsNullOrEmpty(userId))
                    {
                        HttpContext.Session.SetString("UserId", userId);
                    }
                    if (!string.IsNullOrEmpty(userName))
                    {
                        HttpContext.Session.SetString("UserName", userName);
                    }

                    _logger.LogInformation($"User with role {loginResult.Role} logged in successfully");
                    if (loginResult.Role == "Admin")
                    {
                    return RedirectToPage("/AdminPage/Dashboard");
                    }
                    if (loginResult.Role == "Manager")
                    {
                        return RedirectToPage("/ManagerPage/HelperManagement");
                    }
                }
                else
                {
                    _logger.LogWarning("Login result is null");
                    ErrorMessage = "Invalid login response from server";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại sau.";
            }

            ModelState.AddModelError(string.Empty, ErrorMessage ?? "Invalid login attempt.");
            return Page();
        }

    }

    public class LoginApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public LoginResponse Data { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public int UserId { get; set; }
        public DateTime Expiration { get; set; }
        public string DeviceToken { get; set; }
    }
}