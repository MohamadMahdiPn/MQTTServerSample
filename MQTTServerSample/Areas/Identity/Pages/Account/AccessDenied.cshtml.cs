using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MQTTServerSample.Areas.Identity.Pages.Account
{
  
    public class AccessDeniedModel : PageModel
    {
      

        public void OnGet()
        {
            //var userName = User.Identity.Name;
            //_logger.LogError($"User {userName} tried to open page without permission");
        }
    }
}
