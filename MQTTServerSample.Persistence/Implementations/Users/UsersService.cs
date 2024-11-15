using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MQTTServerSample.Application.Contracts.Users;
using MQTTServerSample.Domain.Entities.IdentityModels;
using MQTTServerSample.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MQTTServerSample.Persistence.Implementations.Users;
public class UsersService : IUsersService
{
    #region Constructor

    private readonly UserManager<ApplicationUser> _userManager;

    public UsersService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    #endregion


    #region AddNew

    public async Task<BaseResponse<ApplicationUser>> AddNew(ApplicationUser applicationUser, string password)
    {
        if (await _userManager.FindByNameAsync(applicationUser.UserName) != null)
        {
            return new()
            {
                Errors = new EditableList<string>() { "کاربر تکراری است" },
                IsSucceeded = false
            };
        }
        var newUser = await _userManager.CreateAsync(applicationUser, password);

        return new()
        {
            Errors = newUser.Errors.Select(x => x.Description).ToList(),
            IsSucceeded = newUser.Succeeded
        };
    }

    #endregion

    #region Update

    public async Task<BaseResponse<ApplicationUser>> Update(ApplicationUser applicationUser)
    {
        var user = await _userManager.FindByNameAsync(applicationUser.UserName);
        if (user == null)
        {
            return new()
            {

                Errors = new EditableList<string>() { "کاربر بافت نشد" },
                IsSucceeded = false
            };
        }
        user.FirstName = applicationUser.FirstName;
        user.LastName = applicationUser.LastName;
        user.Email = applicationUser.Email;
        user.PhoneNumber = applicationUser.PhoneNumber;

        var updateUser = await _userManager.UpdateAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        var getGivenNameClaim = claims.FirstOrDefault(x => x.Type == "GivenName");
        if (getGivenNameClaim != null)
        {
            var updateClaim =
                await _userManager.ReplaceClaimAsync(user, getGivenNameClaim, new Claim("GivenName", user.GetDisplayName()));
        }


        return new()
        {
            DataItem = user,
            DataItems = new(),
            Errors = updateUser.Errors?.Select(x => x.Description).ToList() ?? new List<string>(),
            IsSucceeded = updateUser.Succeeded
        };
    }

    public async Task<BaseResponse<ApplicationUser>> CheckPassword(string userId, string currentPass)
    {
        var user = await _userManager.FindByIdAsync(userId);
        BaseResponse<ApplicationUser> result = new()
        {
            DataItem = new(),
            Errors = new() { },
            IsSucceeded = false
        };
        if (user == null)
        {
            result.IsSucceeded = false;
            result.Errors = new EditableList<string>() { "کاربر یافت نشد" };
            return result;
        }

        if (currentPass.IsNullOrEmpty())
        {
            result.IsSucceeded = false;
            result.Errors = new EditableList<string>() { "کلمه عبور خالی است" };
            return result;
        }

        var checkPass = await _userManager.CheckPasswordAsync(user, currentPass);

        result.IsSucceeded = checkPass;
        if (!checkPass)
        {
            result.Errors = new EditableList<string>() { "کلمه عبور فعلی درست نیست" };
        }
        return result;
    }

    public async Task<BaseResponse<ApplicationUser>> ChangePassword(ApplicationUser applicationUser, string currentPassword, string newPassword)
    {
        if (await _userManager.FindByNameAsync(applicationUser.UserName) == null)
        {
            return new()
            {
                Errors = new EditableList<string>() { "کاربر بافت نشد" },
                IsSucceeded = false
            };
        }

        var changePassword = await _userManager.ChangePasswordAsync(applicationUser, currentPassword, newPassword);
        return new()
        {
            Errors = changePassword.Errors.Select(x => x.Description).ToList(),
            IsSucceeded = changePassword.Succeeded
        };
    }


    #endregion

    #region GetUsers

    public BaseResponse<ApplicationUser> GetUsers()
    {
        var users = _userManager.Users.ToList();
        return new()
        {
            DataItems = users,
            IsSucceeded = users != null
        };
    }

    public async Task<BaseResponse<ApplicationUser>> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return new()
        {
            DataItem = user,
            IsSucceeded = user != null
        };
    }


    #endregion

    #region ChangeStatus

    public async Task<BaseResponse<ApplicationUser>> ChangeStatus(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new()
            {
                IsSucceeded = false,
                DataItem = null,
                Errors = new() { "کاربر یافت نشد" }
            };
        }
        user.LockoutEnabled = !user.LockoutEnabled;
        var update = await Update(user);
        return update;
    }

    #endregion

    #region GetUsersInRole
    public async Task<BaseResponse<ApplicationUser>> GetUsersInRole(string roleName)
    {
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        return new()
        {
            DataItems = users.ToList(),
            IsSucceeded = users != null
        };
    }


    #endregion

    #region AddUserToRole

    public async Task<BaseResponse<ApplicationUser>> AddUserToRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new()
            {
                IsSucceeded = false,
                DataItem = null,
                Errors = new() { "کاربر یافت نشد" }
            };
        }
        var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);

        return new()
        {
            Errors = addToRoleResult.Errors.Select(x => x.Description).ToList(),
            IsSucceeded = addToRoleResult.Succeeded
        };
    }

    #endregion

    #region RemoveUserToRole

    public async Task<BaseResponse<ApplicationUser>> RemoveUserToRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new()
            {
                IsSucceeded = false,
                DataItem = null,
                Errors = new() { "کاربر یافت نشد" }
            };
        }
        var addToRoleResult = await _userManager.RemoveFromRoleAsync(user, roleName);

        return new()
        {
            Errors = addToRoleResult.Errors.Select(x => x.Description).ToList(),
            IsSucceeded = addToRoleResult.Succeeded
        };
    }

    #endregion

    #region GetUsersForAddToRole

    public async Task<BaseResponse<ApplicationUser>> GetUsersForAddToRole(string roleName)
    {
        var isInRole = await GetUsersInRole(roleName);
        var allUsers = GetUsers().DataItems;
        var listedUsers = allUsers!.Except(isInRole.DataItems!).ToList();
        return new()
        {
            DataItems = listedUsers
        };
    }

    #endregion


}

