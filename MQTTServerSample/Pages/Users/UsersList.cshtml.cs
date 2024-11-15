using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MQTTServerSample.Application.Contracts.Users;
using MQTTServerSample.Domain.Entities.IdentityModels;

namespace MQTTServerSample.Pages.Users
{
    public class UsersListModel : PageModel
    {
        #region Constructor

        private readonly IUsersService _usersService;

        public UsersListModel(IUsersService usersService)
        {
            _usersService = usersService;
        }

        #endregion

        #region Parameters

        public Paging<ApplicationUser> PagingUsers { get; set; }
        public int? PageNumber = 1;
        //public List<ApplicationUser> Users { get; set; }
        public string CurrentUser { get; set; }

        #endregion


        public void OnGet(int? pageNumber)
        {
            PageNumber = pageNumber == null ? 1 : pageNumber;
            int pageSize = 12;

            var users = _usersService.GetUsers();
            CurrentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PagingUsers = Paging<ApplicationUser>.Create(users.DataItems, pageNumber ?? 1, pageSize);
        }


        public async Task<IActionResult> OnGetChangeState(string userId)
        {
            var user = await _usersService.ChangeStatus(userId);

            return RedirectToPage("UsersList");
        }
    }
}
