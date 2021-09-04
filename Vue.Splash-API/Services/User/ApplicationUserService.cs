using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.User
{
    public class ApplicationUserService: IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindUserById(string id)
        {
            return await _userManager
                .Users
                .Include(usr => usr.Photos)
                .FirstOrDefaultAsync(usr => usr.Id == id);
        }

        public async Task<ApplicationUser> FindUserByUserName(string userName)
        {
            return await _userManager
                .Users
                .Include(usr => usr.Photos)
                .FirstOrDefaultAsync(usr => usr.UserName == userName);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
        {
            return await _userManager.GetRolesAsync(applicationUser);
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user,password);
        }

        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<bool> CheckPassword(string username, string password)
        {
            var user = await FindUserByUserName(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}