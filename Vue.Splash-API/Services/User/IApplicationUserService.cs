using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.User
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> FindUserById(string id);
        Task<ApplicationUser> FindUserByIdentifier(string identifier);
        Task<ApplicationUser> FindUserByUserName(string userName);
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<IList<string>> GetUserRoles(ApplicationUser applicationUser);
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUser(ApplicationUser initialValue, AccountUpdateDto updateDto);
        Task<IdentityResult> UpdatePassword(ApplicationUser user, string currentPassword, string newPassword);
        Task<IdentityResult> DeleteUser(ApplicationUser user);
        Task<bool> CheckPassword(string username, string password);
        Task<string> GenerateResetPasswordToken(ApplicationUser user);
        Task<IdentityResult> ResetUserPassword(ApplicationUser user, string token, string newPassword);
    }
}