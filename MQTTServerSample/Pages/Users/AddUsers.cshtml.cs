using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MQTTServerSample.Application.Contracts.Users;
using MQTTServerSample.Domain.Entities.IdentityModels;
using System.ComponentModel.DataAnnotations;

namespace MQTTServerSample.Pages.Users
{
    public class AddUsersModel : PageModel
    {
        #region Constructor

        private readonly IUsersService _usersService;
        private readonly ILogger<AddUsersModel> _logger;
        public AddUsersModel(IUsersService usersService, ILogger<AddUsersModel> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        #endregion


        #region Parameters

        [BindProperty]
        public InputCreateUserModel UserModel { get; set; }
        public class InputCreateUserModel
        {
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "کلمه عبور یکسان نیست")]
            public string ConfirmPassword { get; set; }
        }


        [BindProperty]
        public List<string> Errors { get; set; } = new();

        #endregion


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    UserName = UserModel.UserName,
                    Email = UserModel.Email,
                    PhoneNumber = UserModel.PhoneNumber,
                    FirstName = UserModel.FirstName,
                    LastName = UserModel.LastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };


                var insertResult = await _usersService.AddNew(newUser, UserModel.Password);

                if (insertResult.IsSucceeded)
                {
                    _logger.LogInformation($"New user {UserModel.UserName} has been created by '{User.Identity.Name}'");
                    return RedirectToPage("UsersList");
                }
                Errors = insertResult.Errors;
            }

            foreach (var modelStateValue in ModelState.Values)
            {
                Errors.Add(modelStateValue.AttemptedValue);
            }
            _logger.LogError($"Failed to create New user {UserModel.UserName} by '{User.Identity.Name}'");
            return Page();

        }
    }
}
