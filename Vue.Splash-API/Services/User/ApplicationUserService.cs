using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Dtos;
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


        public async Task<ApplicationUser> FindUserByIdentifier(string identifier)
        {
            return await _userManager
                .Users
                .Include(usr => usr.Photos)
                .FirstOrDefaultAsync(usr => usr.UserName == identifier || usr.Email == identifier);
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userManager
                .Users
                .Include(usr => usr.Photos)
                .FirstOrDefaultAsync(usr => usr.Email == email);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser applicationUser)
        {
            return await _userManager.GetRolesAsync(applicationUser);
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user,password);
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser initialValue, AccountUpdateDto updateDto)
        {
            initialValue.Email = updateDto.Email;
            initialValue.UserName = updateDto.Username;
            return await _userManager.UpdateAsync(initialValue);
        }

        public async Task<IdentityResult> UpdatePassword(ApplicationUser user,string currentPassword,string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user,currentPassword,newPassword);
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

        public async Task<bool> IsEmailConfirmed(string email)
        {
            var user = await FindUserByEmail(email);
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<string> GenerateResetPasswordToken(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<IdentityResult> ResetUserPassword(ApplicationUser user,string token,string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}