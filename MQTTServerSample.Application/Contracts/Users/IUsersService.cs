using MQTTServerSample.Domain.Entities.IdentityModels;
using MQTTServerSample.Domain.Responses;

namespace MQTTServerSample.Application.Contracts.Users;

public interface IUsersService
{
    Task<BaseResponse<ApplicationUser>> AddNew(ApplicationUser applicationUser, string password);
    Task<BaseResponse<ApplicationUser>> Update(ApplicationUser applicationUser);
    Task<BaseResponse<ApplicationUser>> ChangePassword(ApplicationUser applicationUser, string currentPassword, string newPassword);
    BaseResponse<ApplicationUser> GetUsers();
    Task<BaseResponse<ApplicationUser>> ChangeStatus(string userId);
    Task<BaseResponse<ApplicationUser>> GetUserById(string userId);
    Task<BaseResponse<ApplicationUser>> GetUsersInRole(string roleName);
    Task<BaseResponse<ApplicationUser>> AddUserToRole(string userId, string roleName);
    Task<BaseResponse<ApplicationUser>> RemoveUserToRole(string userId, string roleName);
    Task<BaseResponse<ApplicationUser>> GetUsersForAddToRole(string roleName);
    Task<BaseResponse<ApplicationUser>> CheckPassword(string userId, string currentPass);

}