using AspNetCoreWebApp.UsersApp.Models;
using AspNetCoreWebApp.UsersApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AspNetCoreWebApp.UsersApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApiClientService _apiClientService;

        [BindProperty]
        public User LoginUser { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ApiClientService apiClientService)
        {
            _logger = logger;
            _apiClientService = apiClientService;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (LoginUser != null)
            {
                var serverUser = _apiClientService.ReadAsync(LoginUser.Username).Result;
                ;
                if (serverUser != null)
                {
                    if (LoginUser.Password == serverUser.Password)
                    {
                        return RedirectToPage("Account");
                    }
                }
            }
            return Page();
        }

    }
}