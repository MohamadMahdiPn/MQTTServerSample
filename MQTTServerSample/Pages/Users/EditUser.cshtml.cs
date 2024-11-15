using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using MQTTServerSample.Application.Contracts.Users;
using System.ComponentModel.DataAnnotations;

namespace MQTTServerSample.Pages.Users
{
    public class EditUserModel : PageModel
    {

        #region Constructor

        private readonly IUsersService _usersService;
        private readonly ILogger<EditUserModel> _logger;
        public EditUserModel(IUsersService usersService, ILogger<EditUserModel> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        #endregion


        #region Parameters

        public string Id { get; set; }


        [BindProperty] public List<string> Errors { get; set; } = new List<string>();

        [BindProperty]
        public InputUserModel UserModel { get; set; }

        public class InputUserModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }


        }


        #endregion

        public async Task<IActionResult> OnGet(string Id)
        {
            if (!Id.IsNullOrEmpty())
            {
                var getUser = await _usersService.GetUserById(Id);
                UserModel = new()
                {
                    FirstName = getUser.DataItem.FirstName,
                    LastName = getUser.DataItem.LastName,
                    UserName = getUser.DataItem.UserName,
                    Email = getUser.DataItem.Email,
                    PhoneNumber = getUser.DataItem.PhoneNumber,
                    Id = getUser.DataItem.Id,
                };

            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {

                var targetUser = await _usersService.GetUserById(UserModel.Id);
                targetUser.DataItem.FirstName = UserModel.FirstName;
                targetUser.DataItem.LastName = UserModel.LastName;
                targetUser.DataItem.UserName = UserModel.UserName;
                targetUser.DataItem.Email = UserModel.Email;
                targetUser.DataItem.PhoneNumber = UserModel.PhoneNumber;

                var insertResult = await _usersService.Update(targetUser.DataItem);

                if (insertResult.IsSucceeded)
                {
                    _logger.LogInformation($"New user {UserModel.UserName} has been updated by '{User.Identity.Name}'");
                    return RedirectToPage("UsersList");
                }

            }
            foreach (var modelStateValue in ModelState.Values)
            {
                Errors.Add(modelStateValue.AttemptedValue);
            }
            _logger.LogError($"Failed to update New user {UserModel.UserName} by '{User.Identity.Name}'");
            return Page();

        }
    }
}
