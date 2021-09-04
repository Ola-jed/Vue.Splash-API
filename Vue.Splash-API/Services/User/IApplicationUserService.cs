using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.User
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> FindUserById(string id);
        Task<ApplicationUser> FindUserByUserName(string userName);
        Task<IList<string>> GetUserRoles(ApplicationUser applicationUser);
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);
        Task<IdentityResult> DeleteUser(ApplicationUser user);
        Task<bool> CheckPassword(string username, string password);
    }
}